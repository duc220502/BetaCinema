using BetaCinema.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController(IBillService billService) : ControllerBase
    {
        private readonly IBillService _billService = billService;


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBillById(Guid id)
        {
            var response = await _billService.GetBillById(id);
            return Ok(response);
        }
    }
}
