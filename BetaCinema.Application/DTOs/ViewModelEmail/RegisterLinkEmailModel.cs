using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.ViewModelEmail
{
    public class RegisterLinkEmailModel
    {
        public string UserEmail { get; set; } = default!;
        public string VerifyLink { get; set; } = default!;
    }
}
