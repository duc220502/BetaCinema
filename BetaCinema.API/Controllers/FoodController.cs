using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
using BetaCinema.Domain.Entities.Foods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController(IFoodService foodService) : ControllerBase
    {
        private readonly IFoodService _foodService = foodService;

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFood([FromBody] Request_AddFood rq)
        {
            var food = await _foodService.AddFood(rq);
            return CreatedAtAction(nameof(GetFoodById), "Food", new { id = food?.Data?.Id }, food);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFoodById(Guid id)
        {
            var response = await _foodService.GetFoodById(id);
            return Ok(response);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFood(Guid id, [FromBody] Request_UpdateFood rq)
        {
            var response = await _foodService.UpdateFood(id, rq);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFood(Guid id)
        {
            var response = await _foodService.DeleteFood(id);
            return Ok(response);
        }
    }
}
