using BetaCinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure
{
    public class OtpConfirmationStrategy : IConfirmationMethodStrategy
    {
        public string StrategyName => "OTP";
        public string GenerateToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(4);
            var value = BitConverter.ToUInt32(bytes, 0) % 900_000 + 100_000;
            return value.ToString();
        }

        public TimeSpan GetExpirationTime()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
