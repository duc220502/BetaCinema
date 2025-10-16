using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Users
{
    public class Request_UpdateMyProfile
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; } 
        public string? NumberPhone { get; set; } 
    }
}
