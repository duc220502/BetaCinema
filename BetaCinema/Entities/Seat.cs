namespace BetaCinema.Entities
{
    public class Seat:BaseEntity
    {
        public int Number { get; set; }
        public string Line { get; set; }
        public bool IsActive { get; set; } 

    }
}
