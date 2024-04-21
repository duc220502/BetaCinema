namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseMovieRoom
    {
        public string RoomName { get; set; }
        public IEnumerable<DataResponseRoomSeat> Seats { get; set; }
    }
}
