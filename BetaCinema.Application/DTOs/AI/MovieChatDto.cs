using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.AI
{
    public class MovieChatDto
    {
        public Guid MovieId { get; set; }

        public string Title { get; set; } = default!;

        public string Genre { get; set; } = default!;

        public float DurationMinutes { get; set; }
    }
}
