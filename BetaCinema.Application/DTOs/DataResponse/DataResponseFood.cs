using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseFood
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public double Price { get; set; }

        public string Image { get; set; } = default!;

        public string Description { get; set; } = default!;

        public bool IsActive { get; set; }
    }
}
