namespace BetaCinema.Entities
{
    public class SeatType : BaseEntity
    {
        public string NameType { get; set; }

        public virtual ICollection<Seat> Seats { get; set; }
    }
}
