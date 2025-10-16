using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(IBookingService bookingService ) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking([FromBody] Request_CreateBooking rq)
        {
            var bookingResponse = await _bookingService.CreateBookingAsync(rq);
            return CreatedAtAction(nameof(BillController.GetBillById), "Bill", new { id = bookingResponse?.Data?.BillId }, bookingResponse);
        }


    }
}
