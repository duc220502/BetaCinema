namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_UpdateSchedule
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartAt { get; set; }
    }
}
