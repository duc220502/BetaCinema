using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseSeat
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public LineSeat Line { get; set; }
        public bool IsActive { get; set; }

        public Guid RoomId { get; set; }

        public int SeatStatusId { get; set; }

        public int SeatTypeId { get; set; }
    }
}
