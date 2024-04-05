namespace BetaCinema.Entities
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
