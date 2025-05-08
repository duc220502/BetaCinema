namespace BetaCinema.Entities
{
    public class Schedule : BaseEntity
    {
        public string  Name  { get; set; }
        public DateTime StartAt { get; set; }

        public  DateTime? EndAt { get; set; }

        public string Code { get; set; }


        public bool IsActive { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int RoomId { get; set; }
        public Room Room { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
