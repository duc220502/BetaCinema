namespace BetaCinema.Entities
{
    public class ConfirmEmail:BaseEntity
    {
        public string ConfirmCode { get; set; }
        public DateTime ExpiredTime { get; set; }

        public bool IsConfirm { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
