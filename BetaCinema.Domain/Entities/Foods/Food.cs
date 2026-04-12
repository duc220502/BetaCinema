using BetaCinema.Domain.Entities.Carts;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Interfaces;

namespace BetaCinema.Domain.Entities.Foods
{
    public class Food:BaseEntity
    {
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }

        public string? Image { get; set; }

        public string?  Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<BillFood> BillFoods { get; set; } = new List<BillFood>();
        public virtual ICollection<CartFoodItem> CartFoodItems   { get; set; } = new List<CartFoodItem>();
    }
}
