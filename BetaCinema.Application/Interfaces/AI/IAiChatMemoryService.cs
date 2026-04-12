using BetaCinema.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.AI
{
    public interface IAiChatMemoryService
    {
        Task<IReadOnlyList<ChatMessageDto>> GetHistoryAsync(string sessionId, CancellationToken ct);
        Task AddMessageAsync(string sessionId, ChatMessageDto message, CancellationToken ct);
    }
}
