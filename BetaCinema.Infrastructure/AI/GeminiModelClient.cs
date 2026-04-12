using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.AI
{
    public class GeminiModelClient : IGenerativeAiService
    {
        public string ProviderName => "Gemini";

        private readonly HttpClient _httpClient;
        private readonly GeminiOptions _options;

        public GeminiModelClient(HttpClient httpClient,IOptions<GeminiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
            _httpClient.DefaultRequestHeaders.Remove("x-goog-api-key");
            _httpClient.DefaultRequestHeaders.Add("x-goog-api-key", _options.ApiKey);
        }

        public async Task<string> ChatAsync(
            string systemPrompt,
            IReadOnlyList<ChatMessageDto> history,
            string userMessage,
            CancellationToken ct)
        {
            var sb = new StringBuilder();
            await foreach (var chunk in StreamChatAsync(systemPrompt, history, userMessage, ct))
            {
                sb.Append(chunk);
            }
            return sb.ToString();
        }

        public async IAsyncEnumerable<string> StreamChatAsync(
            string systemPrompt,
            IReadOnlyList<ChatMessageDto> history,
            string userMessage,
            [EnumeratorCancellation] CancellationToken ct)
        {
            var contents = new List<object>();

            foreach (var msg in history)
            {
                contents.Add(new
                {
                    role = msg.Role == "assistant" ? "model" : "user",
                    parts = new[] { new { text = msg.Content } }
                });
            }

            contents.Add(new
            {
                role = "user",
                parts = new[] { new { text = userMessage } }
            });

            var body = new
            {
                systemInstruction = new
                {
                    parts = new[] { new { text = systemPrompt } }
                },
                contents
            };

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"models/{_options.Model}:generateContent");

            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            using var response = await _httpClient.SendAsync(request, ct);
            var raw = await response.Content.ReadAsStringAsync(ct);

            response.EnsureSuccessStatusCode();

            using var doc = JsonDocument.Parse(raw);
            var root = doc.RootElement;

            var text = TryExtractText(root);
            if (string.IsNullOrWhiteSpace(text))
            {
                yield return "Xin lỗi, hiện tôi chưa thể tạo câu trả lời phù hợp.";
                yield break;
            }

            yield return text;
        }

        private static string TryExtractText(JsonElement root)
        {
            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
                return string.Empty;

            var candidate = candidates[0];
            if (!candidate.TryGetProperty("content", out var content))
                return string.Empty;

            if (!content.TryGetProperty("parts", out var parts))
                return string.Empty;

            var sb = new StringBuilder();

            foreach (var part in parts.EnumerateArray())
            {
                if (part.TryGetProperty("text", out var textEl))
                    sb.Append(textEl.GetString());
            }

            return sb.ToString();
        }

    }
}
