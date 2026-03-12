using BetaCinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Security
{
    public class OtpService : IOtpService
    {
        public string GenerateNumericCode(int length = 6)
        {
            var bytes = RandomNumberGenerator.GetBytes(length);
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)('0' + (bytes[i] % 10));
            }
            return new string(chars);
        }
    }
}
