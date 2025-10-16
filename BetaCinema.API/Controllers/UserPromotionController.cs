using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPromotionController(IUserPromotionService userPromotionService) : ControllerBase
    {
        private readonly IUserPromotionService _userPromotionService = userPromotionService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserPromotion([FromBody] Request_AddUserPromotion rq)
        {
            var userPromotion = await _userPromotionService.AddUserPromotionAsync(rq);
            return CreatedAtAction(nameof(GetUserPromotionById), "userPromotion", new { id = userPromotion?.Data?.Id }, userPromotion);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserPromotionById(Guid id)
        {
            var response = await _userPromotionService.GetUserPromotionByIdAsync(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserPromotion(Guid id, [FromBody] Request_UpdateUserPromotion rq)
        {
            var response = await _userPromotionService.UpdateUserPromotion(id, rq);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUserPromotion(Guid id)
        {
            var response = await _userPromotionService.DeleteUserPromotion(id);
            return Ok(response);
        }
    }
}
