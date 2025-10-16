using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class ConfirmEmail:BaseEntity
    {
        public string ConfirmCode { get; set; } = default!;
        public DateTime ExpiredTime { get; set; }

        public bool IsConfirm { get; set; }

        public CodePurpose Purpose { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
