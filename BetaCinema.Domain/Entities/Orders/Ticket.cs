using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class Ticket : BaseEntity
    {
        public string Code { get; set; } = default!;
        public decimal PriceTicket { get; set; }

        public bool IsActive { get; set; }

        public Guid ScheduleId { get; set; }
        public virtual Schedule? Schedule { get; set; }

        public Guid SeatId { get; set; }
        public virtual Seat? Seat { get; set; }
        public virtual ICollection<BillTicket> BillTickets { get; set; } = new List<BillTicket>();
    }
}
