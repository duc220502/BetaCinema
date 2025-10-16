using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataResponse
{
    public class DataResponseRoom
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int Capacity { get; set; }

        public string TypeRoom { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; }

        public Guid CinemaId { get; set; }
    }
}
