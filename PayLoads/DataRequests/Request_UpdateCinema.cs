using System.Xml.Linq;

namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_UpdateCinema
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }
        public bool IsActive { get; set; }

    }
}
