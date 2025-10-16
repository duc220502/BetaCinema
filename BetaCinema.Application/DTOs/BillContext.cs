using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class BillContext
    {
        public Guid UserId { get; set; }

        public decimal SubTotal { get; set; }

        public List<PreparedTicketDto> Tickets { get; set; } = default!;

        public List<PreparedFoodDto> Foods { get; set; } = default!;
    }
}
