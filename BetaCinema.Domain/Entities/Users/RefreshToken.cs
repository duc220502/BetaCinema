using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiredTime { get; set; }
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
