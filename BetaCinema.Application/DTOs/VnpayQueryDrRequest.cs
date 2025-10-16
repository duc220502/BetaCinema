using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class VnpayQueryDrRequest
    {
        public string vnp_RequestId { get; set; } = default!;
        public string vnp_Version { get; set; } = "2.1.0";
        public string vnp_Command { get; set; } = "querydr";
        public string vnp_TmnCode { get; set; } = default!;
        public string vnp_TxnRef { get; set; } = default!;
        public string vnp_OrderInfo { get; set; } = default!;
        public string vnp_TransactionDate { get; set; } = default!; 
        public string vnp_CreateDate { get; set; } = default!;
        public string vnp_IpAddr { get; set; } = "127.0.0.1";
        public string? vnp_TransactionNo { get; set; }    // optional
        public string vnp_SecureHash { get; set; } = default!;


        public Dictionary<string, string> ToDictionary() => new()
        {
            ["vnp_RequestId"] = vnp_RequestId,
            ["vnp_Version"] = vnp_Version,
            ["vnp_Command"] = vnp_Command,
            ["vnp_TmnCode"] = vnp_TmnCode,
            ["vnp_TxnRef"] = vnp_TxnRef,
            ["vnp_OrderInfo"] = vnp_OrderInfo,
            ["vnp_TransactionDate"] = vnp_TransactionDate,
            ["vnp_CreateDate"] = vnp_CreateDate,
            ["vnp_IpAddr"] = vnp_IpAddr,
            ["vnp_TransactionNo"] = vnp_TransactionNo ?? "",
            ["vnp_SecureHash"] = vnp_SecureHash
        };
    }
}
