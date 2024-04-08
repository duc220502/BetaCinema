using System.Globalization;
using System.Text.RegularExpressions;

namespace BetaCinema.Handle
{
    public class InputHelper
    {
        public static bool checkNull(params string[] strings)
        {
            foreach (string str in strings)
            {
                if (String.IsNullOrEmpty(str))
                    return true;
            }
            return false;
        }
        public static string ToTitleCase(string input)
        {
            CultureInfo cultureInfo = CultureInfo.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            return textInfo.ToTitleCase(input);
        }

        public static bool IsValidEmail(string email)
        {

            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";


            Match match = Regex.Match(email, pattern);


            return match.Success;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(0[1-9])+([0-9]{8})\b$";

            Match match = Regex.Match(phoneNumber, pattern);
            return match.Success;
        }
    }
}
