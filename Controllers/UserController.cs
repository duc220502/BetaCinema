using BetaCinema.PayLoads.DataRequests;
using BetaCinema.Services.Implement;
using BetaCinema.Services.Interface;
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
        private readonly IUserSevice _iUserService;

        public UserController(IUserSevice iUserService)
        {
            _iUserService = iUserService;
        }
        [HttpPut("changepassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromForm] Request_ChangPassword rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var userId = int.Parse(userIdClaim.Value);
            var result =  await _iUserService.ChangPassword(userId,rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
        [HttpPut("resetpassword")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ResetPassword([FromForm]int id , [FromForm]string newPassword)
        {
        
            var result = await _iUserService.NewPassword(id, newPassword);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("newpassword")]

        [Authorize]
        public async Task<IActionResult> NewPassword([FromForm] string newPassword)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var userId = int.Parse(userIdClaim.Value);

            var result = await _iUserService.NewPassword(userId, newPassword);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
    }
}
