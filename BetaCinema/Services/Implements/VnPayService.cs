using Azure;
using BetaCinema.Handle;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;

namespace BetaCinema.Services.Implements
{
    public class VnPayService : BaseService, IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<VnPaymentResponseModel> _responseObject;
        public VnPayService(IConfiguration configuration, ResponseObject<VnPaymentResponseModel> responseObject)
        {
            _configuration = configuration;
            _responseObject = responseObject;
        }

        public string CreatePaymentUrl(HttpContext context, Request_VnPayment rq)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();
            var billCr = _context.Bills.FirstOrDefault(b => b.Id == rq.OrderId);
            var percent = _context.Promotions.FirstOrDefault(b => b.Id == billCr.PromotionId).Percent;
            vnpay.AddRequestData("vnp_Version", _configuration["VnPay:Version"]);
            vnpay.AddRequestData("vnp_Command", _configuration["VnPay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((billCr.TotalMoney-(billCr.TotalMoney*(percent/100))) * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _configuration["VnPay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _configuration["VnPay:Locale"]);

            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + rq.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:PaymentBackReturnUrl"]);

            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày
            vnpay.AddResponseData("vnp_BillId", billCr.Id.ToString());
            var paymentUrl = vnpay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);
            return paymentUrl;

        }

        public ResponseObject<VnPaymentResponseModel> PaymentExecute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach(var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
                
            }
            var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
            var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            var billId = Convert.ToInt64(vnpay.GetResponseData("vnp_BillId"));

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VnPay:HashSecret"]);
            if (!checkSignature)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Không thành công", new VnPaymentResponseModel
                {
                    Success = false,
                });
            if (vnp_ResponseCode != "00")
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, $"Lỗi thanh toán VN Pay: {vnp_ResponseCode}", new VnPaymentResponseModel
                {
                    Success = false,
                });
            EmailService emailService = new EmailService();
            emailService.SendEmail("duc220502@gmail.com", "Thanh toán vnpay", "thành công");

            var billCr = _context.Bills.FirstOrDefault(x => x.Id == billId);
            var promotionCr = _context.Promotions.FirstOrDefault(x => x.Id == billCr.PromotionId);

            billCr.IsActive = false;
            billCr.BillStatusId = 2;
            _context.Bills.Update(billCr);
            
            promotionCr.Quantity = promotionCr.Quantity - 1;
            _context.Promotions.Update(promotionCr);

            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Thành công", new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "VnPay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            });

        }
    }
}
