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
    public class UserController : ControllerBase
    {
        private readonly IUserService _IUserService;

        public UserController()
        {
            _IUserService = new UserService();
        }

        [HttpPut("/api/user/ChangePassword")]
        public IActionResult ChangePassword([FromForm] string oldPass, [FromForm] string newPass)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var result = _IUserService.ChangPassword(userId, oldPass, newPass);
            if (result == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpPost("/api/user/ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var result = _IUserService.ForgotPassword(userId);
            if (result == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpPut("/api/user/NewPassword")]
        public IActionResult NewPassword([FromForm] string code, [FromForm] string newPass)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var result = _IUserService.NewPassword(code, newPass);
            if (result == null)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
