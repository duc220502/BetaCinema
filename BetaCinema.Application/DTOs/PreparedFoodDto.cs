using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class PreparedFoodDto
    {
        public Guid FoodId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string FoodName { get; set; } = default!; 
    }
}
