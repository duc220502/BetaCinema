namespace BetaCinema.PayLoads.DataRequests
{
    public class Request_RenewAccessToken
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
