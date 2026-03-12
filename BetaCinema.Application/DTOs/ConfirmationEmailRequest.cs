using BetaCinema.Application.Enums;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class ConfirmationEmailRequest
    {
        public string Token { get; set; } = default!;
        public string UserEmail { get; set; } = default!;

        public CodePurpose Purpose { get; set; }
        public ConfirmationMethod Method { get; set; }

        public string? CallbackUrl { get; set; }
    }
}
