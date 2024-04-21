namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseBill
    {
        public string MovieName { get; set; }
        public string CinemaName { get; set; }
        public string CinemaAddress { get; set; }
        public IEnumerable<DataResponseBillFood> OrderedFoods { get; set; }
        public IEnumerable<DataResponseBillTicket> OrderedTickets { get; set; }

        public double TotalAmountBeforeDiscount { get; set; }
        public double TotalAmountAfterDiscount { get; set; }
    }
}
