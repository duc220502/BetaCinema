using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseTicketInBill
    {
        public Guid Id { get; set; }
        public string SeatPosition { get; set; } = default!;
        public decimal Price { get; set; }
    }
}
