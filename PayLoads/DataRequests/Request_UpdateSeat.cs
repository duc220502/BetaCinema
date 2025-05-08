using BetaCinema.Enum;

namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_UpdateSeat
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public LineSeat Line { get; set; }
    }
}
