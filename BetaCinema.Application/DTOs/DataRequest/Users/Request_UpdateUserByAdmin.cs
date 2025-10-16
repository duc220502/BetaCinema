using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest.Users
{
    public class Request_UpdateUserByAdmin
    {
        public string? FullName { get; set; }
        public int? RoleId { get; set; }

        public int? UserStatusId { get; set; }
    }
}
