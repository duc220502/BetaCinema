using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases.Users;
using BetaCinema.Shared.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CinemaController(ICinemaService cinemaService) : ControllerBase
    {
        private readonly ICinemaService _cinemaService = cinemaService;
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCinema([FromBody] Request_AddCinema rq)
        {
            var cinema = await _cinemaService.AddCinema(rq);
            return CreatedAtAction(nameof(GetCinemaById), "Cinema", new { id = cinema?.Data?.Id }, cinema);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCinemaById(Guid id)
        {
            var response = await _cinemaService.GetCinemaById(id);
            return Ok(response);
        }

        [HttpGet()]
        public async Task<IActionResult> GetCinemas([FromQuery] Pagination pagination)
        {
            var response = await _cinemaService.GetCinemas(pagination);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCinema(Guid id, [FromBody] Request_UpdateCinema rq)
        {
            var response = await _cinemaService.UpdateCinema(id, rq);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCinema(Guid id)
        {
            var response = await _cinemaService.DeleteCinema(id);
            return Ok(response);
        }
    }
}
