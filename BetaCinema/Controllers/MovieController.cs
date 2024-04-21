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
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _IMovieService;

        public MovieController()
        {
            _IMovieService = new MovieService();
        }

        [HttpGet("/api/movie/Get_MovieOutStanding")]
        //[Authorize]
        public IActionResult Get_MovieOutStanding()
        {
           /* var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }*/

            var response = _IMovieService.get_MovieOutStanding();
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }


        [HttpGet("/api/movie/Get_MovieDetail/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult Get_MovieDetail(int id)
        {
            /*var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }*/

            var response = _IMovieService.get_MovieOutStanding();
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPost("/api/movie/AddMovie")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddMovie([FromForm] Request_AddMovie rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IMovieService.CreateMovie(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPut("/api/movie/UpdateMovie/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateFood(int id, [FromForm] Request_UpdateMovie rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IMovieService.UpdateMovie(id, rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPatch("/api/movie/RemoveMovie/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveMovie(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _IMovieService.DeleteMovie(id);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
