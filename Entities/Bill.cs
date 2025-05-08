namespace BetaCinema.Entities
{
    public class Bill : BaseEntity
    {
        public double? TotalMoney { get; set; }
        public string TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string? Name { get; set; }

        public DateTime? UpdateTime { get; set; }

        public bool IsActive { get; set; }

        public int BillStatusId { get; set; }
        public BillStatus BillStatus { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<BillTicket> BillTickets { get; set; }
        public virtual ICollection<BillFood> BillFoods { get; set; }

        public virtual ICollection<BillPromotion> BillPromotions { get; set; }


    }
}
