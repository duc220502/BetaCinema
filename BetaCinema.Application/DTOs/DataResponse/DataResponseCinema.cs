using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseCinema
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;
         
        public string Description { get; set; } = default!;

        public bool IsActive { get; set; }

    }
}
