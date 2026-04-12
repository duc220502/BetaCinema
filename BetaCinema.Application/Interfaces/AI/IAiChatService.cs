using BetaCinema.Application.DTOs.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.AI
{
    public interface IAiChatService
    {
        IAsyncEnumerable<string> StreamAnswerAsync(AiChatRequest request, CancellationToken ct);
    }

}
