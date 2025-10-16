using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class VnpayQueryDrResponse
    {
        public string? vnp_ResponseId { get; set; }
        public string? vnp_Command { get; set; }
        public string? vnp_ResponseCode { get; set; }
        public string? vnp_Message { get; set; }
        public string? vnp_TmnCode { get; set; }
        public string? vnp_TxnRef { get; set; }
        public string? vnp_Amount { get; set; }
        public string? vnp_BankCode { get; set; }
        public string? vnp_PayDate { get; set; }
        public string? vnp_TransactionNo { get; set; }
        public string? vnp_TransactionType { get; set; }
        public string? vnp_TransactionStatus { get; set; }
        public string? vnp_OrderInfo { get; set; }
        public string? vnp_PromotionCode { get; set; }
        public string? vnp_PromotionAmount { get; set; }
        public string? vnp_SecureHash { get; set; }

        public Dictionary<string, string> AsDict() => new()
        {
            ["vnp_ResponseId"] = vnp_ResponseId ?? "",
            ["vnp_Command"] = vnp_Command ?? "",
            ["vnp_ResponseCode"] = vnp_ResponseCode ?? "",
            ["vnp_Message"] = vnp_Message ?? "",
            ["vnp_TmnCode"] = vnp_TmnCode ?? "",
            ["vnp_TxnRef"] = vnp_TxnRef ?? "",
            ["vnp_Amount"] = vnp_Amount ?? "",
            ["vnp_BankCode"] = vnp_BankCode ?? "",
            ["vnp_PayDate"] = vnp_PayDate ?? "",
            ["vnp_TransactionNo"] = vnp_TransactionNo ?? "",
            ["vnp_TransactionType"] = vnp_TransactionType ?? "",
            ["vnp_TransactionStatus"] = vnp_TransactionStatus ?? "",
            ["vnp_OrderInfo"] = vnp_OrderInfo ?? "",
            ["vnp_PromotionCode"] = vnp_PromotionCode ?? "",
            ["vnp_PromotionAmount"] = vnp_PromotionAmount ?? "",
            ["vnp_SecureHash"] = vnp_SecureHash ?? ""
        };
    }
}
