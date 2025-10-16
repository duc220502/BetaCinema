using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public static class VnpaySignature
    {
        public static readonly string[] QueryDrRequestOrder =
        {
            "vnp_RequestId","vnp_Version","vnp_Command","vnp_TmnCode","vnp_TxnRef",
            "vnp_TransactionDate","vnp_CreateDate","vnp_IpAddr","vnp_OrderInfo"
        };

        public static readonly string[] QueryDrResponseOrder =
        {
        "vnp_ResponseId","vnp_Command","vnp_ResponseCode","vnp_Message","vnp_TmnCode",
        "vnp_TxnRef","vnp_Amount","vnp_BankCode","vnp_PayDate","vnp_TransactionNo",
        "vnp_TransactionType","vnp_TransactionStatus","vnp_OrderInfo","vnp_PromotionCode", "vnp_PromotionAmount"
        };

        public static string JoinValues(string[] order, IReadOnlyDictionary<string, string> src)
        => string.Join("|", order.Select(k => src.GetValueOrDefault(k) ?? string.Empty));

        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var b in hashValue)
                {
                    hash.Append(b.ToString("x2"));
                }
            }
            return hash.ToString();
        }

        public static bool TimingSafeEqualsHex(string aHex, string bHex)
        {
            if (aHex is null || bHex is null) return false;

           
            aHex = aHex.Trim();
            bHex = bHex.Trim();

           
            if (aHex.Length != bHex.Length) return false;

            
            byte[] aBytes, bBytes;
            try
            {
                aBytes = Convert.FromHexString(aHex);
                bBytes = Convert.FromHexString(bHex);
            }
            catch
            {
                return false; 
            }

            return CryptographicOperations.FixedTimeEquals(aBytes, bBytes);
        }

    }
}
