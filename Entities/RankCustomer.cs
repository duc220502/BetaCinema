namespace BetaCinema.Entities
{
    public class RankCustomer:BaseEntity
    {
        public int MinimumPoint { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Promotion> Promotions { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
