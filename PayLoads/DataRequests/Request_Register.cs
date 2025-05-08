namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_Register
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }

        public string Password { get; set; }
    }
}
