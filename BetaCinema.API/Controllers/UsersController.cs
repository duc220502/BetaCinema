using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.Interfaces;
using BetaCinema.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;



        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid id)
        {
           var response = await _userService.GetUserById(id);
            return Ok(response);
        }
        [HttpGet()]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers([FromQuery] Pagination pagination)
        {
            var response = await _userService.GetUsers(pagination);
            return Ok(response);
        }

        [HttpPut("password-change")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] Request_ChangePassword rq)
        {
            var response = await _userService.ChangePasswordAsync(rq);
            return Ok(response);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMyProfile()
        {
            var response = await _userService.GetMyProfile();
            return Ok(response);
        }


        [HttpPatch("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] Request_UpdateMyProfile rq)
        {
            var response = await _userService.UpdateMyProfile(rq);
            return Ok(response);
        }
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserByAdmin(Guid id,[FromBody] Request_UpdateUserByAdmin rq)
        {
            var response = await _userService.UpdateUserByAdmin(id,rq);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _userService.DeleteUser (id);
            return Ok(response); 
        }
    }
}
