using BetaCinema.Application.Common;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Enums;
using BetaCinema.Infrastructure.Configuration;
using BetaCinema.Infrastructure.Payments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService , IConfiguration configuration , IOptions<VnpayConfig> vnpayConfigOption) : ControllerBase
    {

        private readonly IPaymentService _paymentService = paymentService;
        private readonly IConfiguration _configuration = configuration;
        private readonly VnpayConfig _vnpayConfig = vnpayConfigOption.Value;

        [HttpPost("initiate/{billId}")]
        [Authorize]
        public async Task<IActionResult> InitiatePayment(Guid billId, [FromBody] PaymentMethod paymentMethod)
        {
            var result = await _paymentService.InitiatePaymentAsync(billId, paymentMethod);
            return Ok(result);
        }

        [HttpGet("vnpay-return")]
        public IActionResult VnpayReturn()
        {
            var queryString = Request.Query;
            var responseCode = queryString["vnp_ResponseCode"];
            var orderInfo = queryString["vnp_OrderInfo"];

            var frontendReturnUrl = _vnpayConfig.FrontendReturnUrl; 

            var isValidSignature = VnpayHelper.ValidateSignature(_vnpayConfig, queryString.ToDictionary(q => q.Key, q => q.Value.ToString()));
            if (!isValidSignature)
            {
                return Redirect($"{frontendReturnUrl}?success=false&reason=invalid_signature");
            }

            if (responseCode == "00") 
            {
                return Redirect($"{frontendReturnUrl}?success=true");
            }

            return Redirect($"{frontendReturnUrl}?success=false&errorCode={responseCode}");
        }

        [HttpGet("vnpay-ipn")]
        public async Task<IActionResult> VnpayIpn()
        {
            var callbackData = Request.Query.ToDictionary(x => x.Key, x => x.Value.ToString());

            var result = await _paymentService.ProcessVnpayIpnAsync(callbackData);

            return Ok(result);
        }


        [HttpPost("confirm-cash-payment/{billId}")]
        [Authorize(Roles = "Staff, Admin")]
        public async Task<IActionResult> ConfirmCashPayment(Guid billId)
        {
            var result = await _paymentService.ProcessCashPaymentConfirmationAsync(billId); 
                                                                              
            return Ok(result);
        }
    }
}
