using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class BillPromotion:BaseEntity
    {
        public Guid PromotionId { get; set; }
        public virtual Promotion? Promotion { get; set; }

        public Guid BillId { get; set; }
        public virtual Bill? Bill { get; set; }
    }
}
