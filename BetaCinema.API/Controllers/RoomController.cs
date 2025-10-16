
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController(IRoomService roomService) : ControllerBase
    {
        private readonly IRoomService _roomService = roomService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom([FromBody] Request_AddRoom rq)
        {
            var room = await _roomService.AddRoom(rq);
            return CreatedAtAction(nameof(GetRoomById), "Room", new { id = room?.Data?.Id }, room);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(Guid id)
        {
            var response = await _roomService.GetRoomById(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCinema(Guid id, [FromBody] Request_UpdateRoom rq)
        {
            var response = await _roomService.UpdateRoom(id, rq);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var response = await _roomService.DeleteRoom(id);
            return Ok(response);
        }
    }
}
