using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.AI
{
    public class AiChatToolResult
    {
        public string ToolName { get; set; } = default!;

        public string JsonResult { get; set; } = default!;
    }
}
