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

        public string CreateEmailBody(string token, string userEmail)
        {
            var resetLink = $"https://yourwebsite.com/reset-password?token={token}";
            return $"Chào {userEmail},<br/>Vui lòng nhấp vào link sau để đặt lại mật khẩu: <a href='{resetLink}'>Đặt lại mật khẩu</a>";
        }

        public string GenerateToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        }

        public TimeSpan GetExpirationTime()
        {
            return TimeSpan.FromHours(1);
        }
    }
}
