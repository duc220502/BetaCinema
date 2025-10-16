using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Seats
{
    public class Request_AddSeat
    {
        public int Number { get; set; }
        public string Line { get; set; } = default!;
        public Guid RoomId { get; set; }
        public int SeatTypeId { get; set; }

    }
}
