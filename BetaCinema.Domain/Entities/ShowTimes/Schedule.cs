using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class Schedule : BaseEntity
    {
        public string Name { get; set; } = default!;
        public DateTime StartAt { get; set; }

        public  DateTime EndAt { get; set; }

        public string Code { get; set; } = default!;


        public bool IsActive { get; set; }

        public Guid MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        public Guid RoomId { get; set; }
        public virtual Room? Room { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    }
}
