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
    public class BillController : ControllerBase
    {
        private readonly IBillService _IBillService;

        public BillController()
        {
            _IBillService = new BillService();
        }

        [HttpPost("/api/bill/AddBill")]
        [Authorize]
        public IActionResult AddBill([FromBody] Request_AddBill rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var userId = int.Parse(userIdClaim.Value);
            var response = _IBillService.CreateBill(userId,rq);
            if (response.status != StatusCodes.Status200OK)
                return StatusCode(response.status, new { message = response.Message });

            return Ok(response);
        }
    }
}
