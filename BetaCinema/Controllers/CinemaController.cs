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
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _ICinemaService;

        public CinemaController()
        {
            _ICinemaService = new CinemaService();
        }

        [HttpPost("/api/cinema/AddCinema")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCinema([FromBody] Request_AddCinema rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ICinemaService.CreatCinema(rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }

        [HttpPut("/api/cinema/UpdateCinema/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCinema( int id,[FromForm] Request_UpdateCinema rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ICinemaService.UpdateCinema(id,rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }


        [HttpPatch("/api/cinema/RemoveCinema/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoveCinema(int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var response = _ICinemaService.DeleteCinema(id);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
