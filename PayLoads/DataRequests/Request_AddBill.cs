namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_AddBill
    {
        public IEnumerable<Request_BillFood> BillFoods { get; set; }
        public IEnumerable<Request_BillTicket> BillTickets { get; set; }

        public IEnumerable<Request_BillPromotion> BillPromotions { get; set; }

    }
}
