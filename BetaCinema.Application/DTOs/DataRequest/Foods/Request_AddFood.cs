using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Foods
{
    public class Request_AddFood
    {
        public string Name { get; set; } = default!;
        public decimal? Price { get; set; }

        public string? Image { get; set; }

        public string? Description { get; set; }
    }
}
