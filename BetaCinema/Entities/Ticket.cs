namespace BetaCinema.Entities
{
    public class Ticket:BaseEntity
    {
        public string Code { get; set; }
        public double PriceTicket { get; set; }
        public bool IsActive { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public IEnumerable<BillTicket> BillTickets { get; set; }
    }
}
