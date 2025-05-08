namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_AddTicket
    {
        public double PriceTicket { get; set; }
        public int SeatId { get; set; }

        public int ScheduleId { get; set; }
    }
}
