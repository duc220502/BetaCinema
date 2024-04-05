namespace BetaCinema.Entities
{
    public class RankCustomer:BaseEntity
    {
        public int Point { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool  IsActive { get; set; }
    }
}
