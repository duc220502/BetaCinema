using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Users
{
    public class Request_ChangePassword
    {
        public string OldPass { get; set; } = default!;
        public string NewPass { get; set; } = default!;
    }
}
