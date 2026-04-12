using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public class AiChatMemoryService(IConnectionMultiplexer mux, IConfiguration config) : IAiChatMemoryService
    {
        private readonly IDatabase _db =  mux.GetDatabase();
        private readonly TimeSpan _ttl = TimeSpan.FromDays(7);

        private static string BuildKey(string sessionId) => $"ai-chat:{sessionId}";

        public async Task<IReadOnlyList<ChatMessageDto>> GetHistoryAsync(string sessionId, CancellationToken ct)
        {
            var value = await _db.StringGetAsync(BuildKey(sessionId));
            if (value.IsNullOrEmpty)
                return Array.Empty<ChatMessageDto>();

            return JsonSerializer.Deserialize<List<ChatMessageDto>>(value!) ?? new List<ChatMessageDto>();
        }

        public async Task AddMessageAsync(string sessionId, ChatMessageDto message, CancellationToken ct)
        {
            var key = BuildKey(sessionId);

            var current = await _db.StringGetAsync(key);
            var history = current.IsNullOrEmpty
                ? new List<ChatMessageDto>()
                : JsonSerializer.Deserialize<List<ChatMessageDto>>(current!) ?? new List<ChatMessageDto>();

            history.Add(message);

            // giữ history ngắn để tiết kiệm token
            if (history.Count > 20)
                history = history.TakeLast(20).ToList();

            var json = JsonSerializer.Serialize(history);
            await _db.StringSetAsync(key, json, _ttl);
        }
    }
}
