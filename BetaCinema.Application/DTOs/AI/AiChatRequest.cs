using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.AI
{
    public class AiChatRequest 
    {
        public string SessionId { get; set; } = default!;
        public string Message { get; set; } = default!;
    }
}
