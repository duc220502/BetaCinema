namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseTicket
    {
        public double PriceTicket { get; set; }

        public string StatusActive { get; set; }

        public string? StatusLocked { get; set; }

        public DateTime? LockedTime { get; set; }

        public string? LockByUserName { get; set; }

        public DateTime  ScheudleTime { get; set; }

        public int SeatNumber { get; set; }

        public string RoomName { get; set; }

    }
}
