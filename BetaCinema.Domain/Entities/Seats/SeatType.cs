using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Seats
{
    public class SeatType 
    {
        public int Id { get; set; }
        public string NameType { get; set; } = default!;

        public decimal  Surcharge { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}
