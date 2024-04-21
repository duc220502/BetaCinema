namespace BetaCinema.Payloads.DataRequest
{
    public class Request_AddBill
    {
        public int ScheduleId { get; set; }
        public IEnumerable<Request_BillFoodItem>? BillFoods { get; set; }
        public IEnumerable<Request_BillTicketItem>? BillTickets { get; set; }
        
    }
}
