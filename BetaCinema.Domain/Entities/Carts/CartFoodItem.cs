using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Carts
{
    public class CartFoodItem : BaseEntity
    {
        public int  Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CartId { get; set; }

        public virtual Cart? Cart { get; set; }

        public Guid FoodId { get; set; }

        public virtual Food? Food { get; set; }
    }
}
