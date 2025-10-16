using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Orders
{
    public class Bill : BaseEntity
    {
        public string TradingCode { get; set; } = default!;
        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string? Note { get; set; }

        public DateTime? ExpireAt { get; set; }

        public decimal? SubTotal { get; set; }      
        public decimal? DiscountAmount { get; set; } 

        public decimal? TotalMoney { get; set; }

        public bool IsActive => BillStatusId == (int)Domain.Enums.BillStatus.PendingPayment ||
                                BillStatusId == (int)Domain.Enums.BillStatus.Paid ||
                                BillStatusId == (int)Domain.Enums.BillStatus.Failed;

        public int PaymentAttemptCount { get; set; }

        public int BillStatusId { get; set; }
        public virtual BillStatus? BillStatus { get; set; }

        public int? PaymentMethodId { get; set; }

        public virtual PaymentMethod? PaymentMethod { get; set; }

        public string? PaymentGatewayTransactionId { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<BillTicket> BillTickets { get; set; } = new List<BillTicket>();
        public virtual ICollection<BillFood> BillFoods { get; set; } = new List<BillFood>();

        public virtual ICollection<BillPromotion> BillPromotions { get; set; } = new List<BillPromotion>();


    }
}
