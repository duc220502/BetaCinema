using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.AI
{
    public class AiChatService(IAiContextResolver modelResolver,    IAiChatMemoryService memory,
    IAiPromptBuilder aiPromptBuilder) : IAiChatService
    {
        private readonly IAiContextResolver _modelResolver = modelResolver;
        private readonly IAiChatMemoryService _memory = memory;
        private readonly IAiPromptBuilder _aiPromptBuilder = aiPromptBuilder;

        /*public async IAsyncEnumerable<string> StreamAnswerAsync(
            AiChatRequest request,
            [EnumeratorCancellation] CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId))
                throw new ArgumentException("SessionId is required.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Message is required.");

            var history = await _memory.GetHistoryAsync(request.SessionId, ct);

            var systemPrompt = await BuildSystemPromptAsync(request.Message, ct);

            var assistantBuilder = new StringBuilder();

            await foreach (var delta in _openAi.StreamChatAsync(systemPrompt, history, request.Message, ct))
            {
                assistantBuilder.Append(delta);
                yield return delta;
            }

            await _memory.AddMessageAsync(request.SessionId, new ChatMessageDto { Role = "user",Content = request.Message}, ct);
            await _memory.AddMessageAsync(request.SessionId, new ChatMessageDto { Role = "assistant",Content = assistantBuilder.ToString()}, ct);
        }

        private async Task<string> BuildSystemPromptAsync(string message, CancellationToken ct)
        {
            var lower = message.ToLowerInvariant();
            var context = new StringBuilder();

            context.AppendLine("""
            Bạn là trợ lý AI của BetaCinema.
            Nguyên tắc:
            - Chỉ trả lời dựa trên dữ liệu được cung cấp bên dưới.
            - Không bịa phim, lịch chiếu, ghế, khuyến mãi, chính sách.
            - Nếu thiếu dữ liệu thì nói rõ rằng chưa có đủ thông tin.
            - Trả lời bằng tiếng Việt, rõ ràng, ngắn gọn, thân thiện.
            """);

            // Policy
            if (lower.Contains("hoàn vé") ||
                lower.Contains("đổi vé") ||
                lower.Contains("thành viên") ||
                lower.Contains("thanh toán") ||
                lower.Contains("ưu đãi"))
            {
                var policies = await _policyService.SearchRelevantPoliciesAsync(message, ct);
                context.AppendLine("Dữ liệu chính sách BetaCinema:");
                foreach (var p in policies)
                {
                    context.AppendLine($"- {p.Title}: {p.Content}");
                }
            }

            // Now showing
            if (lower.Contains("đang chiếu") || lower.Contains("hôm nay có phim gì"))
            {
                var movies = await _movieQueryService.GetNowShowingAsync(DateOnly.FromDateTime(DateTime.Now), ct);
                context.AppendLine("Danh sách phim đang chiếu:");
                foreach (var m in movies)
                {
                    context.AppendLine($"- {m.Title} | Thể loại: {m.Genre} | Thời lượng: {m.DurationMinutes} phút");
                }
            }

            // Showtimes after time
            if ((lower.Contains("suất") || lower.Contains("chiếu")) &&
                (lower.Contains("sau 8h") || lower.Contains("sau 20h")))
            {
                var movieName = ExtractMovieName(message);
                if (!string.IsNullOrWhiteSpace(movieName))
                {
                    var showtimes = await _movieQueryService.GetShowtimesByMovieAfterAsync(
                        movieName,
                        DateOnly.FromDateTime(DateTime.Now),
                        new TimeOnly(20, 0),
                        ct);

                    context.AppendLine($"Suất chiếu của phim {movieName} sau 20:00:");
                    foreach (var s in showtimes)
                    {
                        context.AppendLine($"- {s.MovieTitle} | {s.StartTime:HH:mm dd/MM/yyyy} | {s.RoomName}");
                    }
                }
            }

            return context.ToString();
        }

        private static string ExtractMovieName(string message)
        {
            // Bản đơn giản để demo production-like
            // Sau này thay bằng intent parser hoặc function calling
            var markers = new[] { "phim", "của phim" };
            foreach (var marker in markers)
            {
                var idx = message.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
                if (idx >= 0)
                {
                    var value = message[(idx + marker.Length)..].Trim();
                    if (!string.IsNullOrWhiteSpace(value))
                        return value;
                }
            }

            // Nếu người dùng hỏi kiểu "Dune có suất nào sau 8h"
            var firstWord = message.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return firstWord ?? string.Empty;
        }*/

        public async IAsyncEnumerable<string> StreamAnswerAsync(
        AiChatRequest request,
        [EnumeratorCancellation] CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.SessionId))
                throw new ArgumentException("SessionId is required.");

            if (string.IsNullOrWhiteSpace(request.Message))
                throw new ArgumentException("Message is required.");

            var history = await _memory.GetHistoryAsync(request.SessionId, ct);
            var systemPrompt = await _aiPromptBuilder.BuildSystemPromptAsync(request.Message, ct);
            var modelClient = _modelResolver.GetDefault();

            var assistantBuilder = new StringBuilder();

            await foreach (var delta in modelClient.StreamChatAsync(systemPrompt, history, request.Message, ct))
            {
                assistantBuilder.Append(delta);
                yield return delta;
            }

            await _memory.AddMessageAsync(
                request.SessionId,
                new ChatMessageDto { Role = "user", Content = request.Message },
                ct);

            await _memory.AddMessageAsync(
                request.SessionId,
                new ChatMessageDto { Role = "assistant", Content = assistantBuilder.ToString() },
                ct);
        }
    }
}
