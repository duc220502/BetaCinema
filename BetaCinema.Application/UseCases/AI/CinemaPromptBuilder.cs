using BetaCinema.Application.Interfaces.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.AI
{
    public class CinemaPromptBuilder : IAiPromptBuilder
    {
        private readonly IAiPolicyService _policyService;
        private readonly IChatMovieQueryService _movieQueryService;

        public CinemaPromptBuilder(
            IAiPolicyService policyService,
            IChatMovieQueryService movieQueryService)
        {
            _policyService = policyService;
            _movieQueryService = movieQueryService;
        }

        public async Task<string> BuildSystemPromptAsync(string message, CancellationToken ct)
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

            if (lower.Contains("đang chiếu") || lower.Contains("hôm nay có phim gì"))
            {
                var movies = await _movieQueryService.GetNowShowingAsync(DateOnly.FromDateTime(DateTime.Now), ct);
                context.AppendLine("Danh sách phim đang chiếu:");
                foreach (var m in movies)
                {
                    context.AppendLine($"- {m.Title} | Thể loại: {m.Genre} | Thời lượng: {m.DurationMinutes} phút");
                }
            }

            return context.ToString();
        }
    }
}
