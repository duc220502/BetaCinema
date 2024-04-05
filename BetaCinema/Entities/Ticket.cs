namespace BetaCinema.Entities
{
    public class Ticket:BaseEntity
    {
        public string Code { get; set; }
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; }
    }
}
