namespace BetaCinema.Entities
{
    public class Room:BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int Capacity { get; set; }

        public string RoomType { get; set; }
        public string  Description { get; set; }
        public bool IsActive { get; set; }

        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
