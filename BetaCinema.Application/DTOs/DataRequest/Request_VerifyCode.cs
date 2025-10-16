using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.DataRequest
{
    public class Request_VerifyCode
    {
        public Guid userId { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
