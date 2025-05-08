using BetaCinema.PayLoads.DataRequests;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnPayService;
        public VnpayController(IVnpayService vnPayService)
        {

            _vnPayService = vnPayService;
        }
        [HttpPost("createpaymenturl")]
        public IActionResult CreatePaymentUrlVnpay([FromBody]Request_VnpayPayment model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);
            Console.WriteLine("URL gửi sang VNPay: " + url);
            return Redirect(url);
        }
        [HttpGet("paymentcallback")]
        public IActionResult PaymentCallbackVnpay()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            return new JsonResult(response);
        }

    }
}
