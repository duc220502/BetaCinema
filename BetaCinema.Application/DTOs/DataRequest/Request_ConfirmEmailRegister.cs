using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest
{
    public class Request_ConfirmEmailRegister
    {
        public Guid UserId { get; set; }

        public string Code { get; set; } = default!;
    }
}
