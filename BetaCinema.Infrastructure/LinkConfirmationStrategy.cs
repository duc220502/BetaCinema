using BetaCinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure
{
    public class LinkConfirmationStrategy : IConfirmationMethodStrategy
    {
        public string StrategyName => "LINK";

        public string GenerateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        public TimeSpan GetExpirationTime()
        {
            return TimeSpan.FromMinutes(15);
        }
    }
}
