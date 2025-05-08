using BetaCinema.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_AddSeat
    {
        public int Number { get; set; }

        public LineSeat Line { get; set; }
        public int RoomId { get; set; }
        public int SeatStatusId { get; set; }

        public int SeatTypeId { get; set; }
    }
}
