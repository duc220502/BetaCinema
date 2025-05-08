namespace BetaCinema.Entities
{
    public class Food:BaseEntity
    {
        public string Name  { get; set; }
        public double Price { get; set; }

        public string? Image { get; set; }

        public string?  Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<BillFood> BillFoods { get; set; }
    }
}
