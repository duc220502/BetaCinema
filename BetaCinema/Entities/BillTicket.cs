namespace BetaCinema.Entities
{
    public class BillTicket:BaseEntity
    {
        public int Quantity { get; set; }
        public int BillId { get; set; }
        public Bill Bill { get; set; }

        public int TicketId{ get; set; }
        public Ticket Ticket { get; set; }
    }
}
