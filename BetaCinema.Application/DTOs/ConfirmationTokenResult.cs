using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class ConfirmationTokenResult
    {
        public string Token { get; set; } = default!;
        public DateTimeOffset Expiration { get; set; }
        public string Method { get; set; } = default!;

        public string CodePurpose { get; set; } = default!;
    }
}
