namespace BetaCinema.Entities
{
    public class Schedule:BaseEntity
    {
        public double Price { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
