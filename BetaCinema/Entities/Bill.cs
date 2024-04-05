namespace BetaCinema.Entities
{
    public class Bill:BaseEntity
    {
        public double TotalMoney { get; set; }
        public string  TradingCode { get; set; }
        public DateTime CreateTime { get; set; }
        public string Name { get; set; }
        public DateTime? UpdateTime { get; set; }
        public bool IsActive { get; set; }

    }
}
