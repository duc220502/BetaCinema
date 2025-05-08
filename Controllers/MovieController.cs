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
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _iMoviveService;

        public MovieController(IMovieService iMovieService)
        {
            _iMoviveService = iMovieService;
        }

        [HttpPost("createmovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMovie([FromForm] Request_AddMovie rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iMoviveService.AddMovie(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("updatemovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom([FromForm] Request_UpdateMovie rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iMoviveService.UpdateMovie(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpDelete("deletemovie")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMovie([FromQuery] int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iMoviveService.DeleteMovie(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpGet("detailmovie")]
        public async Task<IActionResult> DetailMovie([FromQuery] int id)
        {
            var result = await _iMoviveService.DetailMovie(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpGet("getlistmovieofcinema")]
        public async Task<IActionResult> GetListMovieOfCinema([FromQuery] int id, [FromQuery] Pagination pagination)
        {
            var result = await _iMoviveService.ListMoviesOfCinemas(pagination,id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

    }
}
