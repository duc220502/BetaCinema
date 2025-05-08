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
    public class BillController : ControllerBase
    {
        private readonly IBillService _iBillService;

        public BillController(IBillService iBillService)
        {
            _iBillService = iBillService;
        }

        [HttpPost("createbill")]
        [Authorize()]
        public async Task<IActionResult> CreateBill([FromBody] Request_AddBill rq)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Không xác thực được người dùng.");
            }
            var userId = int.Parse(userIdClaim.Value);
            var result = await _iBillService.AddBill(userId,rq);
            if (result.status != StatusCodes.Status200OK)
                return StatusCode(result.status, new { message = result.Message });

            return Ok(result);
        }
    }
}
