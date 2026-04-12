using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Cart
{
    public class AddFoodToCartRequest
    {
        public Guid FoodId { get; set; }
        public int Quantity { get; set; }
    }
}
