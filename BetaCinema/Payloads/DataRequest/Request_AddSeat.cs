namespace BetaCinema.Payloads.DataRequest
{
    public class Request_AddSeat
    {
        public int Number { get; set; }
        public string Line { get; set; }
        public int SeatStatusId { get; set; }
        public int SeatTypeId { get; set; }
    }
}
