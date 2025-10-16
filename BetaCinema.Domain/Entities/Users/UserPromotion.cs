using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Users
{
    public class UserPromotion:BaseEntity
    {
        public int Quantity { get; set; }
        public bool IsActive { get; set; }

        public Guid PromotionId { get; set; }
        public virtual Promotion? Promotion { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
