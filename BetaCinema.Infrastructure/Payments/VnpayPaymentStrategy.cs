using BetaCinema.Application.DTOs;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces.PaymentStrategies;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.X9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace BetaCinema.Infrastructure.Payments
{
    public class VnpayPaymentStrategy(IOptions<VnpayConfig> vnpayConfigOption , IHttpContextAccessor httpContextAccessor , IHttpClientFactory httpClientFactory , IVnpayClient vnpayClient) : IPaymentStrategy
    {
        private readonly VnpayConfig _vnpayConfig = vnpayConfigOption.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IVnpayClient _vnpayClient = vnpayClient;
        public string StrategyCode => Domain.Enums.PaymentMethod.VNPAY.ToString() ;

        public Task<PaymentConfirmationResult> ConfirmPaymentAsync(Dictionary<string, string> callbackData)
        {
            var isValid = VnpayHelper.ValidateSignature(_vnpayConfig, callbackData);
            if (!isValid)
                return Task.FromResult(new PaymentConfirmationResult { IsSuccess = false, ErrorMessage = "Chữ ký không hợp lệ." });

            var responseCode = callbackData.GetValueOrDefault("vnp_ResponseCode");
            var parts = callbackData.GetValueOrDefault("vnp_TxnRef")?.Split('_')!;
            

            var billId = Guid.Parse(parts[0] ?? throw new BadRequestException("Không lấy được giá  trị TxnRef"));
            var amount = decimal.Parse(callbackData.GetValueOrDefault("vnp_Amount") ?? throw new BadRequestException("Không lấy được giá  trị Amount")) / 100;
            var transId = callbackData.GetValueOrDefault("vnp_TransactionNo");
            var payDate = callbackData.GetValueOrDefault("vnp_PayDate");
            var txnRef = callbackData.GetValueOrDefault("vnp_TxnRef");


            if (responseCode == "00")
                return Task.FromResult(new PaymentConfirmationResult
                { IsSuccess = true, BillId = billId, Amount = amount, PaymentGatewayTransactionId = transId , PayDate = payDate , TxnRef = txnRef });

            return Task.FromResult(new PaymentConfirmationResult
            { IsSuccess = false, BillId = billId, ErrorMessage = $"Thanh toán VNPAY thất bại. Mã lỗi: {responseCode}" });
        }

        public Task<PaymentInitiationResult> InitiatePaymentAsync(Bill bill)
        {
            var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");
            var paymentUrl = VnpayHelper.CreatePaymentUrl(_vnpayConfig, context, bill.Id, bill.TotalMoney??throw new BadRequestException("Gía trị TotalMoney không hợp lệ"),bill.PaymentAttemptCount);

            return Task.FromResult(new PaymentInitiationResult() { IsSuccess = true, Message = "URL VNPAY được tạo thành công.", QrCodeData = null, RedirectUrl = paymentUrl });
        }

        public async Task<bool> VerifyTransactionOnGatewayAsync(Guid billId, decimal amount, string gatewayTransactionId , string payDate , string txnRef)
        {
            //var context = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is not available.");
            /*var pay = new SortedDictionary<string, string>(new VnpayComparer());
            var requestId = Guid.NewGuid().ToString(); 
            var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string version = "2.1.0";
            string command = "querydr";
            string tmnCode = _vnpayConfig.TmnCode;
            string orderInfo = $"Kiem tra gd don hang {billId}";
            string ipAddr = "127.0.0.1";*/


            var req = new VnpayQueryDrRequest
            {
                vnp_RequestId = Guid.NewGuid().ToString("N"),
                vnp_TmnCode = _vnpayConfig.TmnCode,
                vnp_TxnRef = txnRef,
                vnp_TransactionDate = payDate,
                vnp_CreateDate = DateTime.Now.ToString("yyyyMMddHHmmss"),
                vnp_OrderInfo = $"Kiem tra gd don hang {billId}",
                vnp_TransactionNo = gatewayTransactionId
            };

            var res = await _vnpayClient.QueryDrAsync(req, _vnpayConfig.HashSecret);
            if (res == null) return false;

            // 3) Đối chiếu amount + status
            if (!long.TryParse(res.vnp_Amount, out var amt)) return false;
            var responseAmount = amt / 100m;
            return res.vnp_TransactionStatus == "00" && responseAmount == amount;


            /*var body = new Dictionary<string, string>
            {
                ["vnp_RequestId"] = requestId,
                ["vnp_Version"] = version,
                ["vnp_Command"] = command,
                ["vnp_TmnCode"] = tmnCode,
                ["vnp_TxnRef"] = txnRef,
                ["vnp_OrderInfo"] = orderInfo,
                ["vnp_TransactionDate"] = payDate,
                ["vnp_CreateDate"] = createDate,
                ["vnp_IpAddr"] = ipAddr
            };

            var dataForHash = string.Join("|", new[]
            {
                    requestId, version, command, tmnCode, txnRef, payDate, createDate, ipAddr, orderInfo
            });

            var secureHash = VnpaySignature.HmacSHA512(_vnpayConfig.HashSecret, dataForHash);

            if (!string.IsNullOrEmpty(gatewayTransactionId))
            {
                body["vnp_TransactionNo"] = gatewayTransactionId; 
            }
            body["vnp_SecureHash"] = secureHash;


            var client = _httpClientFactory.CreateClient("VNPAY");
            var queryDrUrl = "https://sandbox.vnpayment.vn/merchant_webapi/api/transaction";
            using var requestContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(queryDrUrl, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseBody))
                return false;

            Dictionary<string, string>? responseData = null;
            var trimmed = responseBody.Trim();
            if (trimmed.StartsWith("{"))
            {
                responseData = JsonSerializer.Deserialize<Dictionary<string, string>>(trimmed);
            }
            else
            {
                
                responseData = trimmed
                    .Split('&', StringSplitOptions.RemoveEmptyEntries)
                    .Select(part => part.Split('=', 2))
                    .Where(a => a.Length == 2)
                    .ToDictionary(a => a[0], a => Uri.UnescapeDataString(a[1]));
            }

            if (responseData == null)
                return false;

            var isResponseSignatureValid = VnpayHelper.ValidateQueryDrResponseSignature(_vnpayConfig, responseData);
            if (!isResponseSignatureValid)
                return false;

            var responseAmountStr = responseData.GetValueOrDefault("vnp_Amount", "0");
            if (!decimal.TryParse(responseAmountStr, out var responseAmountLong))
                return false;

            var responseAmount = responseAmountLong / 100m;
            var transactionStatus = responseData.GetValueOrDefault("vnp_TransactionStatus");

            return transactionStatus == "00" && amount == responseAmount;*/

           
        }
    }
}
