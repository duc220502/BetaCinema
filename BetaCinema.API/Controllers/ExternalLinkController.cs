using BetaCinema.Application.DTOs.Auth.Requests;
using BetaCinema.Application.Interfaces.Auths;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExternalLinkController(IExternalLinkingService externalLinkingService) : ControllerBase
    {
        private readonly IExternalLinkingService _externalLinkingService = externalLinkingService;

        [HttpPost("auth/external/confirm-link")]
        public async Task<IActionResult> ConfirmLink([FromBody] ConfirmExternalLinkRequest req, CancellationToken ct)
        {
            var response = await _externalLinkingService.ConfirmLinkAsync(req, ct);
            return Ok(response);
        }

    }
}
