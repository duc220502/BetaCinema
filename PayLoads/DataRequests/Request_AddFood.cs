namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_AddFood
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }
    }
}
