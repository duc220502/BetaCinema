using BetaCinema.Application.DTOs.Auth.Requests;
using BetaCinema.Application.Interfaces.Auths;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/auth/external")]
    [ApiController]
    public class ExternalLinkController(IExternalLinkingService externalLinkingService) : ControllerBase
    {
        private readonly IExternalLinkingService _externalLinkingService = externalLinkingService;

        [HttpPost("confirm-link")]
        public async Task<IActionResult> ConfirmLink([FromBody] ConfirmExternalLinkRequest req, CancellationToken ct)
        {

            Console.WriteLine($"LinkingToken: {req.LinkingToken}");
            Console.WriteLine($"Otp: {req.Otp}");
            var response = await _externalLinkingService.ConfirmLinkAsync(req, ct);
            return Ok(response);
        }


        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendExternalLinkOtpRequest req, CancellationToken ct)
        {
            var response = await _externalLinkingService.ResendOtpAsync(req, ct);
            return Ok(response);
        }
    }
}
