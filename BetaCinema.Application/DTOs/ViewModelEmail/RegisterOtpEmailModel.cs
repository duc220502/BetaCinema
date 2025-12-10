using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.ViewModelEmail
{
    public class RegisterOtpEmailModel
    {
        public string UserEmail { get; set; } = default!;
        public string Otp { get; set; } = default!;
    }
}
