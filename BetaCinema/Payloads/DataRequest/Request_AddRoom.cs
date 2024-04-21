using BetaCinema.Entities;

namespace BetaCinema.Payloads.DataRequest
{
    public class Request_AddRoom
    {
        public int Capacity { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? CinemaId { get; set; }
        //public IEnumerable<Request_AddSeat> Seats { get; set; }
    }
}
 