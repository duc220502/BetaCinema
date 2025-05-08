namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_AddSchedule
    {
        public string Name { get; set; }
        public DateTime StartAt { get; set; }

        public int MovieId { get; set; }
        public int RoomId { get; set; }

    }
}
