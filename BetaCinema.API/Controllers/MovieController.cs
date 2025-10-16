using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.DTOs.DataRequest.Movies;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController(IMovieService movieService) : ControllerBase
    {
        private readonly IMovieService _movieService = movieService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie([FromBody] Request_AddMovie rq)
        {
            var movie = await _movieService.AddMovie(rq);
            return CreatedAtAction(nameof(GetMovieById), "Movie", new { id = movie?.Data?.Id }, movie);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(Guid id)
        {
            var response = await _movieService.GetMovieById(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMovie(Guid id, [FromBody] Request_UpdateMovie rq)
        {
            var response = await _movieService.UpdateMovie(id, rq);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie(Guid id)
        {
            var response = await _movieService.DeleteMovie(id);
            return Ok(response);
        }
    }
}
