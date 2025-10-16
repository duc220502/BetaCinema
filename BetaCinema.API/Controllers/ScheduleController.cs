using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Schedule;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController(IScheduleService scheduleService) : ControllerBase
    {
        private readonly IScheduleService _scheduleService = scheduleService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchedule([FromBody] Request_AddSchedule rq)
        {
            var schedule = await _scheduleService.AddSchedule(rq);
            return CreatedAtAction(nameof(GetScheduleById), "Schedule", new { id = schedule?.Data?.Id }, schedule);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetScheduleById(Guid id)
        {
            var response = await _scheduleService.GetScheduleById(id);
            return Ok(response);
        }

        [HttpGet("available-slots")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAvailableSlots([FromQuery] Guid roomId,[FromQuery] Guid movieId,[FromQuery] DateTime date)
        {
            var availableSlots = await _scheduleService.GetAvailableSlotsAsync(roomId, movieId, date);
            return Ok(availableSlots);
        }

        [HttpGet("{scheduleId}/seats")]
        public async Task<IActionResult> GetSeatsBySchedule(Guid scheduleId)
        {
            var response = await _scheduleService.GetSeatsForScheduleAsync(scheduleId);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSchedule(Guid id, [FromBody] Request_UpdateSchedule rq)
        {
            var response = await _scheduleService.UpdateSchedule(id, rq);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchedule(Guid id)
        {
            var response = await _scheduleService.DeleteSchedule(id);
            return Ok(response);
        }
    }
}
