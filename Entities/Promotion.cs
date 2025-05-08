using BetaCinema.Enum;

namespace BetaCinema.Entities
{
    public class Promotion:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PromotionType Type { get; set; }
        public int? Quantity { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<BillPromotion> BillPromotion { get; set; }
        public virtual ICollection<UserPromotion> UserPromotions { get; set; }

        public int? RankCustomerId { get; set; }
        public RankCustomer RankCustomer { get; set; }
    }
}
