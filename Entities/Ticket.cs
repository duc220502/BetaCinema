namespace BetaCinema.Entities
{
    public class Ticket : BaseEntity
    {
        public string Code { get; set; }
        public double PriceTicket { get; set; }

        public bool IsActive { get; set; }

        public bool? IsLocked { get; set; }

        public DateTime? LockedTime { get; set; }

        public int? LockByUserId { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }
        public virtual ICollection<BillTicket> BillTickets { get; set; }
    }
}
