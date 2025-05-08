namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseBill
    {
        public double TotalMoney { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string StatusActive { get; set; }

        public string BillStatus { get; set; }

        public string UserName { get; set; }

        public string MovieName { get; set; }

        public string CinemaName { get; set; }
        public string RoomName { get; set; }

        public DateTime StartAt { get; set; }
        public DateTime  EndAt { get; set; }

        public IEnumerable<DataResponseBillTicket> BillTickets { get; set; }

        public IEnumerable<DataResponseBillFood> BillFoods { get; set; }

        public IEnumerable<DataResponsePromotion> Promotions { get; set; }
    }
}
