using BetaCinema.Application.DTOs.AI;
using BetaCinema.Application.Interfaces.AI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BetaCinema.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiChatController(IAiChatService aiChatService,ILogger<AiChatController> logger) : ControllerBase
    {
        private readonly IAiChatService _aiChatService = aiChatService;
        private readonly ILogger<AiChatController> _logger = logger;

        [HttpPost("test")]
        public async Task<IActionResult> TestChat(
            [FromBody] AiChatRequest request,
            CancellationToken ct)
        {
            if (request is null)
                return BadRequest("Request body is required.");

            var sb = new StringBuilder();

            await foreach (var chunk in _aiChatService.StreamAnswerAsync(request, ct))
            {
                sb.Append(chunk);
            }

            return Ok(new
            {
                request.SessionId,
                request.Message,
                Answer = sb.ToString()
            });
        }

        [HttpPost("stream")]
        public async Task StreamChat(
            [FromBody] AiChatRequest request,
            CancellationToken ct)
        {
            if (request is null)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                await Response.WriteAsync("Request body is required.", ct);
                return;
            }

            Response.StatusCode = StatusCodes.Status200OK;
            Response.ContentType = "text/plain; charset=utf-8";

            await foreach (var chunk in _aiChatService.StreamAnswerAsync(request, ct))
            {
                if (ct.IsCancellationRequested)
                    break;

                await Response.WriteAsync(chunk, ct);
                await Response.Body.FlushAsync(ct);
            }
        }
    }
}
