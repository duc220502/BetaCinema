using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOS.DataRequest.Users
{
    public class Request_Register
    {
        public string UserName { get; set; } = default!;
        public string? FullName { get; set; }
        public string Email { get; set; } = default!;
        public string NumberPhone { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
