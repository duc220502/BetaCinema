using BetaCinema.Entities;
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
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _iSeatService;

        public SeatController(ISeatService iSeatService)
        {
            _iSeatService = iSeatService;
        }

        [HttpPost("createseat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSeat([FromForm] Request_AddSeat rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iSeatService.AddSeat(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("updateseat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSeat([FromForm] Request_UpdateSeat rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iSeatService.UpdateSeat(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }


        [HttpDelete("deleteseat")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeat([FromQuery] int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iSeatService.DeleteSeat(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpGet("getlistseatofroom")]
        public async Task<IActionResult> GetListMovieOfCinema([FromQuery] int id, [FromQuery] Pagination pagination)
        {
            var result = await _iSeatService.ListSeatOfRooms(pagination, id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
    }
}
