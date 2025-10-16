using BetaCinema.Application.DTOs.DataRequest.Promotions;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController(IPromotionService promotionService) : ControllerBase
    {
        private readonly IPromotionService _promotionService = promotionService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePromotion([FromBody] Request_AddPromotion rq)
        {
            var promotion = await _promotionService.AddPromotion(rq);
            return CreatedAtAction(nameof(GetPromotionById), "Promotion", new { id = promotion?.Data?.Id }, promotion);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPromotionById(Guid id)
        {
            var response = await _promotionService.GetPromotionById(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePromotion(Guid id, [FromBody] Request_UpdatePromotion rq)
        {
            var response = await _promotionService.UpdatePromotion(id, rq);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePromotion(Guid id)
        {
            var response = await _promotionService.DeletePromotion(id);
            return Ok(response);
        }
    }
}
