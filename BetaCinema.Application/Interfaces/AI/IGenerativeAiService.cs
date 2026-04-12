using BetaCinema.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.AI
{
    public interface IGenerativeAiService
    {
        string ProviderName { get; }
        IAsyncEnumerable<string> StreamChatAsync(string systemPrompt,IReadOnlyList<ChatMessageDto> history,string userMessage,CancellationToken ct);
    }
}
