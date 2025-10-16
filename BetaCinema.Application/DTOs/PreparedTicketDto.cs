using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PreparedTicketDto
    {
        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid SeatId { get; set; }
        public decimal PriceTicket { get; set; }
        public string Code { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
