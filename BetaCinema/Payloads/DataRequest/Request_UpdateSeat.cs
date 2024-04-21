namespace BetaCinema.Payloads.DataRequest
{
    public class Request_UpdateSeat
    {
        public int Number { get; set; }
        public string Line { get; set; }
        public bool IsActive { get; set; }
        public int SeatStatusId { get; set; }
        public int SeatTypeId { get; set; }
    }
}
