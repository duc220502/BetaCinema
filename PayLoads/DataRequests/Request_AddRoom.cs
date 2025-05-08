using BetaCinema.Enum;

namespace BetaCinema.PayLoads.DataRequests
{
    
    public class Request_AddRoom
    {
        public string Name { get; set; }
        public int Capacity { get; set; }

        public RoomType Type { get; set; }
        public string? Description { get; set; }

        public int CinemaId { get; set; }
    }
}
