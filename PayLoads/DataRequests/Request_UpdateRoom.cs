using BetaCinema.Enum;

namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_UpdateRoom
    {
        public int Id { get; init; }
        public string Name { get; set; }
        public int? Capacity { get; set; }

        public RoomType RoomType { get; set; }
        public string Description { get; set; }
    }
}
