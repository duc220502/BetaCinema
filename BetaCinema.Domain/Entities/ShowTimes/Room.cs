using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class Room:BaseEntity
    {
        public string? Name { get; set; }
        public string Code { get; set; } = default!;
        public int Capacity { get; set; }

        public RoomType RoomType { get; set; }
        public string?  Description { get; set; }
        public bool IsActive { get; set; }

        public Guid CinemaId { get; set; }
        public virtual  Cinema? Cinema { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
