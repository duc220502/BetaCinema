using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.AI
{
    public class ShowtimeChatDto
    {
        public Guid ShowtimeId { get; set; }

        public string MovieTitle { get; set; } = default!;

        public DateTime StartTime { get; set; }

        public string RoomName { get; set; } = default!;
    }
}
