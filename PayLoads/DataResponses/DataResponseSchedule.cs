namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseSchedule
    {
        public string Name { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }

        public string StatusActive { get; set; }

        public string MovieName { get; set; }

        public string RoomName { get; set; }

    }
}
