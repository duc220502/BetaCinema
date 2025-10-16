using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Foods
{
    public class Request_FoodItem
    {
        public Guid FoodId { get; set; }

        public int Quantity { get; set; }
    }
}
