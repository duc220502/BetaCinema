using BetaCinema.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public static class VnpayHelper
    {
        public static string CreatePaymentUrl(VnpayConfig vnpayConfig, HttpContext context, Guid orderId, decimal amount ,int attemptCount)
        {
            var tmnCode = vnpayConfig.TmnCode;
            var hashSecret = vnpayConfig.HashSecret;
            var baseUrl = vnpayConfig.BaseUrl;
            var returnUrl = vnpayConfig.ReturnUrl;
            var version = vnpayConfig.Version;
            var command = vnpayConfig.Command;
            var currcode = vnpayConfig.CurrCode;
            var locale = vnpayConfig.Locale;

            var pay = new SortedDictionary<string, string>(new VnpayComparer());

            pay.Add("vnp_Version", version);
            pay.Add("vnp_Command", command);
            pay.Add("vnp_TmnCode", tmnCode);
            pay.Add("vnp_Amount", ((long)(amount * 100)).ToString()); 
            pay.Add("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            pay.Add("vnp_CurrCode", currcode);
            pay.Add("vnp_IpAddr", GetIpAddress(context));
            pay.Add("vnp_Locale", locale);
            pay.Add("vnp_OrderInfo", $"Thanh toan don hang {orderId}");
            pay.Add("vnp_OrderType", "other"); 
            pay.Add("vnp_ReturnUrl", returnUrl);

            string txnRef = (attemptCount > 1) ? $"{orderId}_{attemptCount}" : orderId.ToString();

            pay.Add("vnp_TxnRef", txnRef);

            var data = string.Join("&", pay.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            
            var secureHash = VnpaySignature.HmacSHA512(hashSecret, data);

           
            return $"{baseUrl}?{data}&vnp_SecureHash={secureHash}";
        }

        public static bool ValidateSignature(VnpayConfig vnpayConfig, Dictionary<string, string> responseData)
        {
            
            var vnp_SecureHash = responseData.GetValueOrDefault("vnp_SecureHash", string.Empty);

           
            var pay = new SortedDictionary<string, string>(new VnpayComparer());
            foreach (var (key, value) in responseData)
            {
               
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") && key != "vnp_SecureHash")
                {
                    pay.Add(key, value);
                }
            }

           
            var data = string.Join("&", pay.Select(kvp => $"{kvp.Key}={WebUtility.UrlEncode(kvp.Value)}"));

            var hashSecret = vnpayConfig.HashSecret;
            var checkSum = VnpaySignature.HmacSHA512(hashSecret, data);

            return checkSum.Equals(vnp_SecureHash, StringComparison.OrdinalIgnoreCase);
        }


        public static bool ValidateQueryDrResponseSignature(string hashSecret, Dictionary<string, string> responseData)
        {
            if (!responseData.TryGetValue("vnp_SecureHash", out var secureHash) || string.IsNullOrEmpty(secureHash))
                return false;

            var data = VnpaySignature.JoinValues(VnpaySignature.QueryDrResponseOrder, responseData); 

            var checkSum = VnpaySignature.HmacSHA512((hashSecret ?? string.Empty).Trim(), data);
            return VnpaySignature.TimingSafeEqualsHex(checkSum, secureHash);
        }

        public static string GetIpAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }
            return ipAddress;
        }


        
    }
}
