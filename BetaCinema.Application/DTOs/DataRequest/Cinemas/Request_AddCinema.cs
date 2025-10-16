using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Cinemas
{
    public class Request_AddCinema
    {
        public string Name { get; set; } = default!;

        public string Address { get; set; } = default!;

        public string Description { get; set; } = default!;
    }
}
