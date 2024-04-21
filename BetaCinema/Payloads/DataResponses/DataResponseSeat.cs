using BetaCinema.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseSeat
    {
        public int Number { get; set; }
        public string Line { get; set; }
        public string ActiveStatus { get; set; }

        public string SeatStatus { get; set; }
        public string RoomName { get; set; }
        public string SeatType { get; set; }

    }
}
