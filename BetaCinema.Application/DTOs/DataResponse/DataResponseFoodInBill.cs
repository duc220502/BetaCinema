using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseFoodInBill
    {
        public Guid Id { get; set; }
        public string FoodName { get; set; } = default!;
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
