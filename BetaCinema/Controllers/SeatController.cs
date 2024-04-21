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
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _ISeatService;

        public SeatController()
        {
            _ISeatService = new SeatService();
        }

        [HttpPost("/api/seat/AddSeat")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSeat([FromBody] IEnumerable<Request_AddSeat> rqs, [FromQuery] int roomId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ISeatService.CreateSeat(roomId,rqs);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPut("/api/seat/UpdateSeat/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSeat(int id, [FromForm] Request_UpdateSeat rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ISeatService.UpdateSeat(id, rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPatch("/api/room/RemoveSeat/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveSeat(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ISeatService.DeleteSeat(id);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
