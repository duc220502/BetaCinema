using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Configuration
{
    public class VnpayConfig
    {
        public string TmnCode { get; set; } = default!;
        public string HashSecret { get; set; } = default!;
        public string BaseUrl { get; set; } = default!;
        public string Version { get; set; } = default!;
        public string Command { get; set; } = default!;
        public string CurrCode { get; set; } = default!;
        public string Locale { get; set; } = default!;
        public string ReturnUrl { get; set; } = default!;
        public string IpnUrl { get; set; } = default!;

        public string FrontendReturnUrl { get; set; } = default!;
    }
}
