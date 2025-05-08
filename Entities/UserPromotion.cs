namespace BetaCinema.Entities
{
    public class UserPromotion:BaseEntity
    {
        public int Quantity { get; set; }
        public bool IsActive { get; set; }

        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
