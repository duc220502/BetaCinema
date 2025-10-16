using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class BillTicket:BaseEntity
    {
        public Guid TicketId { get; set; }
        public virtual Ticket? Ticket { get; set; }

        public Guid BillId { get; set; }
        public virtual Bill? Bill { get; set; }
    }
}
