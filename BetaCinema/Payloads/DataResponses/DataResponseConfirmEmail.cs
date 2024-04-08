namespace BetaCinema.Payloads.DataResponses
{
    public class DataResponseConfirmEmail
    {
        public DateTime ExpiredTime { get; set; }
        public string ConfirmCode { get; set; }
        public string StatusConfirm { get; set; }
        public string UserName { get; set; }
    }
}
