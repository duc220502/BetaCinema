namespace BetaCinema.Entities
{
    public class BillPromotion:BaseEntity
    {
        public int PromotionId { get; set; }
        public Promotion Promotion { get; set; }

        public int BillId { get; set; }
        public Bill Bill { get; set; }
    }
}
