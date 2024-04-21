namespace BetaCinema.Payloads.DataRequest
{
    public class Request_UpdateRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
