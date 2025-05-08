namespace BetaCinema.Entities
{
    public class User:BaseEntity
    {
        public string  UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NumberPhone   { get; set; }

        public int Point { get; set; }

        public string Password { get; set; }

        public bool IsActive { get; set; }

        public int RankCustomerId { get; set; }
        public RankCustomer RankCustomer { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public int UserStatusId { get; set; }
        public UserStatus UserStatus { get; set; }


        public virtual ICollection<UserPromotion> UserPromotions { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }

        public virtual ICollection<ConfirmEmail> ConfirmEmails { get; set; }
    }
}
