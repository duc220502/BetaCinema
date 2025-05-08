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
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _iFoodservice;

        public FoodController(IFoodService iFoodService)
        {
            _iFoodservice = iFoodService;
        }


        [HttpPost("createfood")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFood([FromForm] Request_AddFood rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iFoodservice.AddFood(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpPut("updatefood")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFood([FromForm] Request_UpdateFood rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iFoodservice.UpdateFood(rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

        [HttpDelete("deletefood")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFood([FromQuery] int id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }

            var result = await _iFoodservice.DeleteFood(id);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }

    }
}
