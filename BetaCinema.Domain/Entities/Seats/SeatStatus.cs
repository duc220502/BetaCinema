using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Seats
{
    public class SeatStatus
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string NameStatus { get; set; } = default!;

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
