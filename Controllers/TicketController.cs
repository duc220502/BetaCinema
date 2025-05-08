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
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _iTicketService;

        public TicketController(ITicketService iTicketService)
        {
            _iTicketService = iTicketService;
        }

        [HttpPost("createticket")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTicket([FromForm] Request_AddTicket rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iTicketService.AddTicket(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("updateticket")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTicket([FromForm] Request_UpdateTicket rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iTicketService.UpdateTicket(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpDelete("deleteticket")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTicket([FromQuery] int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iTicketService.DeleteTicket(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("lockedticket")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockedTicket([FromForm] Request_LockedTicket rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var userId = int.Parse(userIdClaim.Value);
            var result = await _iTicketService.LockedTicket(userId,rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
    }
}
