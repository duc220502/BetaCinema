using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Carts
{
    public class Cart : BaseEntity
    {
        public bool IsActive { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<CartFoodItem> CartFoodItems { get; set; } = new List<CartFoodItem>();

    }
}
