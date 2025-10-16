using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class User:BaseEntity
    {
        public string?  UserName { get; set; }
        public string? FullName { get; set; }
        public string Email { get; set; } = default!;
        public string NumberPhone { get; set; } = default!;

        public int Point { get; set; }

        public string Password { get; set; } = default!;

        public bool IsActive => UserStatusId == (int)Domain.Enums.UserStatus.Active;

        public int RankCustomerId { get; set; }
        public virtual RankCustomer? RankCustomer { get; set; }

        public int RoleId { get; set; }
        public virtual Role? Role { get; set; }

        public int UserStatusId { get; set; }
        public virtual UserStatus? UserStatus { get; set; }


        public virtual ICollection<UserPromotion> UserPromotions { get; set; } = new List<UserPromotion>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public virtual ICollection<ConfirmEmail> ConfirmEmails { get; set; } = new List<ConfirmEmail>();
        public virtual ICollection<ExternalLogin> ExternalLogins { get; set; } = new List<ExternalLogin>();

    }
}
