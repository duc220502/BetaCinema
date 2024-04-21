using BetaCinema.Payloads.DataRequest;
using BetaCinema.Services.Implements;
using BetaCinema.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnPayController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;

        public VnPayController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpPost("/api/vnpay/create-payment-url")]
        [Authorize]
        public IActionResult CreatePaymentUrl([FromBody] Request_VnPayment request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, request);
            if (paymentUrl == null)
            {
                return BadRequest("Failed to create payment URL");
            }
            return Ok(paymentUrl);
        }

        [HttpPost("/api/vnpay/payment-response")]
        [Authorize]
        public IActionResult HandlePaymentResponse()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
