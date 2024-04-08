namespace BetaCinema.Payloads.DataRequest
{
    public class Request_RenewAccessToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
