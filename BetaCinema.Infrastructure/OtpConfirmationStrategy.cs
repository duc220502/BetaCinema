using BetaCinema.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure
{
    public class OtpConfirmationStrategy : IConfirmationMethodStrategy
    {
        public string StrategyName => "OTP";

        public string CreateEmailBody(string token, string userEmail)
        {
            return $"Chào {userEmail},<br/>Mã OTP để đặt lại mật khẩu của bạn là: <strong>{token}</strong>";
        }

        public string GenerateToken()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public TimeSpan GetExpirationTime()
        {
            return TimeSpan.FromMinutes(5);
        }
    }
}
