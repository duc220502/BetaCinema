using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Configuration
{
    public class MailSettings
    {
        public string DisplayName { get; set; } = default!;
        public string Mail { get; set; } = default!;
        public string Host { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int Port { get; set; } = default!;
    }
}
