using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController(ISeatService seatService) : ControllerBase
    {

        private readonly ISeatService _seatService = seatService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSeat([FromBody] Request_AddSeat rq)
        {
            var seat = await _seatService.AddSeatAsync(rq);
            return CreatedAtAction(nameof(GetSeatById), "Seat", new { id = seat?.Data?.Id }, seat);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatById(Guid id)
        {
            var response = await _seatService.GetSeatByIdAsync(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSeat(Guid id, [FromBody] Request_UpdateSeat rq)
        {
            var response = await _seatService.UpdateSeat(id, rq);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeat(Guid id)
        {
            var response = await _seatService.DeleteSeat(id);
            return Ok(response);
        }

        

    }
}
