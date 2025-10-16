using BetaCinema.Domain.Entities.Seats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class TicketPreparationResult
    {
        public List<PreparedTicketDto> TicketDtos { get; set; } = default!;

        public List<Seat> ValidatedSeats { get; set; } = default!;
    }
}
