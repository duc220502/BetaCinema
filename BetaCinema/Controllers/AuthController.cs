using BetaCinema.Entities;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;

        public AuthController(IAuthService authService)
        {
            _IAuthService = authService;
        }

        [HttpPost("/api/auth/Register")]
        public IActionResult Register([FromForm] Request_Register rq)
        {
            var response = _IAuthService.Register(rq);
            if(response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPost("/api/auth/Login")]
        public IActionResult Login([FromForm] Request_Login rq)
        {
            return Ok(_IAuthService.Login(rq));
        }

        [HttpPost]
        [Route("/api/auth/renew-token")]
        public IActionResult RenewToken(Request_RenewAccessToken rq)
        {
            var result = _IAuthService.RenewAccessToken(rq);
            if (result == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpGet("/api/auth/get_All")]
        [Authorize(Roles = "Admin")]
        public IActionResult getAll([FromForm] Pagination? pagination)
        {
            var result = _IAuthService.get_User(pagination);
            if (result == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpPost("/api/auth/confirm_email")]
        public IActionResult ConfirmEmail(string code)
        {
            try
            {
                var response = _IAuthService.ConfirmEmail(code);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); 
            }
        }
    }
}
