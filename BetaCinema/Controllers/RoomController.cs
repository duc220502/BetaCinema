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
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _IRoomService;

        public RoomController()
        {
            _IRoomService = new RoomService();
        }


        [HttpPost("/api/room/AddRoom")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddRoom([FromForm] Request_AddRoom rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IRoomService.CreateRoom(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }


        [HttpPut("/api/room/UpdateRoom/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateRoom(int id, [FromForm] Request_UpdateRoom rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IRoomService.UpdateRoom(id, rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPatch("/api/room/RemoveRoom/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveRoom(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IRoomService.DeleteRoom(id);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
