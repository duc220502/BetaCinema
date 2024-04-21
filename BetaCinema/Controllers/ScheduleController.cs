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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _IScheduleService;

        public ScheduleController()
        {
            _IScheduleService = new ScheduleService();
        }

        [HttpPost("/api/schedule/AddSchedule")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddSchedule([FromForm] Request_AddSchedule rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IScheduleService.CreateSchedule(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
        [HttpPut("/api/schedule/UpdateSchedule/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateSchedule(int id, [FromForm] Request_UpdateSchedule rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IScheduleService.UpdateSchedule(id, rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
        [HttpPatch("/api/schedule/RemoveSchedule/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveSchedule(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IScheduleService.DeleteSchedule(id);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
