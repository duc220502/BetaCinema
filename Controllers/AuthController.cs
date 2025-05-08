using BetaCinema.PayLoads.DataRequests;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Security.Claims;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;

        public AuthController(IAuthService authService)
        {
            _iAuthService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] Request_Register rq)
        {

            var response = await _iAuthService.Register(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] Request_Login rq)
        {

            var response = await _iAuthService.Login(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPost("renewaccesstoken")]
        public async Task<IActionResult> RenewAccessToken([FromForm] Request_RenewAccessToken rq)
        {

            var response = await _iAuthService.RenewToken(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
