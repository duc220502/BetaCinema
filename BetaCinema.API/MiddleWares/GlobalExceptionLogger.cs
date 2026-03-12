namespace BetaCinema.API.MiddleWares
{
    public sealed class GlobalExceptionLogger(RequestDelegate next, ILogger<GlobalExceptionLogger> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionLogger> _logger = logger;


        public async Task InvokeAsync(HttpContext ctx)
        {
            try
            {
                await _next(ctx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception at {Path}", ctx.Request.Path);
                throw; // để middleware/handler phía sau quyết định response
            }
        }


    }
}
