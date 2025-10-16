using BetaCinema.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public class VnpayClient : IVnpayClient
    {
        private readonly HttpClient _http;
        private static readonly JsonSerializerOptions JsonOpt = new(JsonSerializerDefaults.Web)
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = null, // giữ nguyên tên field
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public VnpayClient(IHttpClientFactory f)
        {
            _http = f.CreateClient("VNPAY"); 
        }

        public async Task<VnpayQueryDrResponse?> QueryDrAsync(VnpayQueryDrRequest req,string hashSecret,CancellationToken ct = default)
        {
           
            var dict = new Dictionary<string, string>
            {
                ["vnp_RequestId"] = req.vnp_RequestId,
                ["vnp_Version"] = req.vnp_Version,
                ["vnp_Command"] = req.vnp_Command,
                ["vnp_TmnCode"] = req.vnp_TmnCode,
                ["vnp_TxnRef"] = req.vnp_TxnRef,
                ["vnp_TransactionDate"] = req.vnp_TransactionDate,
                ["vnp_CreateDate"] = req.vnp_CreateDate,
                ["vnp_IpAddr"] = req.vnp_IpAddr,
                ["vnp_OrderInfo"] = req.vnp_OrderInfo
            };
            var dataForHash = VnpaySignature.JoinValues(VnpaySignature.QueryDrRequestOrder, dict);
            req.vnp_SecureHash = VnpaySignature.HmacSHA512(hashSecret, dataForHash);

            using var content = new StringContent(JsonSerializer.Serialize(req, JsonOpt), Encoding.UTF8, "application/json");
            using var resp = await _http.PostAsync("/merchant_webapi/api/transaction", content, ct);
            var json = await resp.Content.ReadAsStringAsync(ct);
            if (!resp.IsSuccessStatusCode || string.IsNullOrWhiteSpace(json))
                return null;

            var dto = JsonSerializer.Deserialize<VnpayQueryDrResponse>(json, JsonOpt);
            if (dto == null) return null;

            if (!VnpayHelper.ValidateQueryDrResponseSignature(hashSecret, dto.AsDict()))
                return null;

            return dto;
        }
    }
}
