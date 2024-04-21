namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ActiveStatus { get; set; }

        public string CinemaName { get; set; }
    }
}
