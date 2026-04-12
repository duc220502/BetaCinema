using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.AI
{
    public interface IAiPromptBuilder
    {

        Task<string> BuildSystemPromptAsync(string message, CancellationToken ct);
    }
}
