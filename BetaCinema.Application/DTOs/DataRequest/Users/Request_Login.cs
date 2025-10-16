using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Users
{
    public class Request_Login
    {
        public string UserLogin { get; set; } = default!;

        public string Password { get; set; } = default!;
    }
}
