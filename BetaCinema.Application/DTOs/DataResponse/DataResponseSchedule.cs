using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseSchedule 
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public string Code { get; set; } = default!;


        public bool IsActive { get; set; }

        public Guid MovieId { get; set; }

        public Guid RoomId { get; set; }
    }
}
