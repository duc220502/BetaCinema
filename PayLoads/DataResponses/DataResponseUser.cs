using System.Drawing;

namespace BetaCinema.PayLoads.DataResponses
{
    public class DataResponseUser
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }

        public string Active { get; set; }
        public int Point { get; set; }

        public string  RoleName { get; set; }
        public string Status { get; set; }

    }
}
