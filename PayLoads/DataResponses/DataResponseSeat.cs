namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseSeat
    {
        public int Number { get; set; }
        public string Line { get; set; }
        public string StatusActive { get; set; }

        public string NameRoom { get; set; }
        public string SeatStatus { get; set; }
        public string SeatType { get; set; }
    }
}
