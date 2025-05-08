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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _iScheduleService;

        public ScheduleController(IScheduleService iScheduleService)
        {
            _iScheduleService = iScheduleService;
        }

        [HttpPost("createschedule")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchedule([FromForm] Request_AddSchedule rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iScheduleService.AddSchedule(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("updateschedule")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSchedule([FromForm] Request_UpdateSchedule rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iScheduleService.UpdateSchedule(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpDelete("deleteschedule")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule([FromQuery] int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result =  await _iScheduleService.DeleteSchedule(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
    }
}
