using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Schedule
{
    public class Request_AddSchedule 
    {
        public DateTime StartAt { get; set; }

        public Guid MovieId { get; set; }

        public Guid RoomId { get; set; }
    }
}
