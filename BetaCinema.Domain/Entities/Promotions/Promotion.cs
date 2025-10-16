using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;


namespace BetaCinema.Domain.Entities.Promotions
{
    public class Promotion:BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public PromotionScope Scope { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        public decimal? MinBillValue { get; set; }

        public decimal? MaxDiscountValue { get; set; }

        public int UsageLimit { get; set; }
        public int CurrentUsage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }

        
        public int? RankCustomerId { get; set; }
        public virtual RankCustomer? RankCustomer { get; set; }

        public virtual ICollection<BillPromotion> BillPromotions { get; set; } = new List<BillPromotion>();
        public virtual ICollection<UserPromotion> UserPromotions { get; set; } = new List<UserPromotion>();

    }
}
