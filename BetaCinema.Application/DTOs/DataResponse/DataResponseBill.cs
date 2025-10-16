using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseBill
    {
        public Guid BillId { get; set; }
        public string TradingCode { get; set; } = default!;
        public DateTime CreateTime { get; set; }
        public string Status { get; set; } = default!;

        public string MovieName { get; set; } = default!;
        public string RoomName { get; set; } = default!;

        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public List<DataResponseTicketInBill> Tickets { get; set; } = default!;
        public List<DataResponseFoodInBill> Foods { get; set; } = default!;
    }
}
