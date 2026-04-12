using BetaCinema.Application.DTOs.Cart;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private readonly ICartService _cartService = cartService;

        [HttpGet("header")]
        [Authorize]
        public async Task<IActionResult> GetHeaderCart(CancellationToken ct)
        {
            var result = await _cartService.GetHeaderCartAsync(ct);
            return Ok(result);
        }

        [HttpPost("foods")]
        [Authorize]
        public async Task<IActionResult> AddFoodToCart([FromBody] AddFoodToCartRequest request, CancellationToken ct)
        {

            var result = await _cartService.AddFoodToCartAsync(request, ct);
            return Ok(result);
        }

        [HttpDelete("{cartFoodItemId:guid}")]
        [Authorize]
        public async Task<IActionResult> RemoveFoodFromCart([FromRoute] Guid cartFoodItemId, CancellationToken ct)
        {
            var result = await _cartService.RemoveCartItemAsync(cartFoodItemId, ct);
            return Ok(result);
        }


    }
}
