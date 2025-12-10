using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Configuration
{
    public class RedisSettings
    {
        public string Connection { get; set; } = string.Empty;
        public string Instance { get; set; } = string.Empty;
        public int MaxRetry { get; set; } = 3;
        public int ConnectTimeoutMs { get; set; } = 5000;
    }
}
