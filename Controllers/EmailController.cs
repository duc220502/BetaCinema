using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Services.Implement;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _iEmailService;

        public EmailController(IEmailService iEmailService)
        {
            _iEmailService = iEmailService;
        }
        [HttpPost("sendmail")]
        public async Task<IActionResult> SendMail([FromForm] IEnumerable<string> to)
        {



            ConfirmEmail confirmEmail = new ConfirmEmail()
            {
                ConfirmCode = "Duc"+DateTime.Now.Ticks.ToString(),
                ExpiredTime = DateTime.Now.AddMinutes(1),
                IsConfirm = false,
                UserId = 2

            };

            var message = new EmailMessage(to,"Thông báo mail :", $"MÃ xác nhận : {confirmEmail.ConfirmCode}");
            var response = await _iEmailService.SendMail(message);

            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);

        }

        [HttpPut("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromForm] string code)
        { 
            var response = await _iEmailService.ConfirmEmail(code);

            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);

        }

    }
}
