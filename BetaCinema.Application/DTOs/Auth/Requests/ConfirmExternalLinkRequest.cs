using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Auth.Requests
{
    public class ConfirmExternalLinkRequest
    {
        public string LinkingToken { get; set; } = default!;

        public string Otp { get; set; } = default!;


    }
}
