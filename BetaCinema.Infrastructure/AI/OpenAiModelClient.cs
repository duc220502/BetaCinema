using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.AI
{
    public class OpenAiModelClient : IGenerativeAiService
    {
        public string ProviderName => "OpenAI";

        //private readonly IConfiguration _config;
        private readonly HttpClient _httpClient ;
        private readonly OpenAiOptions  _options ;

        public OpenAiModelClient(IConfiguration configuration, HttpClient httpClient, IOptions<OpenAiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeoutSeconds);
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _options.ApiKey);
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
            var input = new List<object>
            {
                new { role = "system", content = systemPrompt }
            };

            foreach (var msg in history)
            {
                input.Add(new { role = msg.Role, content = msg.Content });
            }

            input.Add(new { role = "user", content = userMessage });

            var body = new
            {
                model = _options.Model,
                input,
                stream = true
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "responses");
            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/event-stream"));

            using var response = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                ct);

            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var reader = new StreamReader(stream);

            string? line;
            var dataLines = new List<string>();

            while (!reader.EndOfStream && (line = await reader.ReadLineAsync()) != null)
            {
                ct.ThrowIfCancellationRequested();

                if (line.Length == 0)
                {
                    if (dataLines.Count == 0) continue;

                    var data = string.Join("\n", dataLines);
                    dataLines.Clear();

                    if (data == "[DONE]")
                        yield break;

                    using var doc = JsonDocument.Parse(data);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("type", out var typeEl))
                    {
                        var type = typeEl.GetString();

                        if (type == "response.output_text.delta" &&
                            root.TryGetProperty("delta", out var deltaEl))
                        {
                            yield return deltaEl.GetString() ?? "";
                        }
                        else if (type == "response.completed")
                        {
                            yield break;
                        }
                    }

                    continue;
                }

                if (line.StartsWith("data:", StringComparison.Ordinal))
                {
                    dataLines.Add(line.Substring(5).TrimStart());
                }
            }
        }
    }
}
