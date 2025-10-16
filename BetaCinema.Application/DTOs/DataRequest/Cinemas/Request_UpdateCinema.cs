using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Cinemas
{
    public class Request_UpdateCinema
    {
        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Description { get; set; }
        public bool? IsActive { get; set; }

    }
}
