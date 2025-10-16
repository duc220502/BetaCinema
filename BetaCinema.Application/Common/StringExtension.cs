using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BetaCinema.Application.Common
{
    public static class StringExtension
    {
        public static bool IsValidEmail(this string email)
        {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }
        public static bool IsValidPhoneNumber(this string phone)
        {
            return Regex.IsMatch(phone, @"^(0|\+84)[0-9]{9}$");
        }
    }
}
