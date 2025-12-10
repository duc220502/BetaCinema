using BetaCinema.Application.Interfaces.Catching;

namespace BetaCinema.API.MiddleWares
{
    public class RateLimitMiddleware(RequestDelegate next)
    {
        
        public async Task InvokeAsync(HttpContext ctx, IRateLimiter limiter)
        {
            var ok = await limiter.HitAsync(ctx.Request.Path, ctx.Connection.RemoteIpAddress!.ToString(), 100, TimeSpan.FromMinutes(1));
            if (!ok)
            {
                ctx.Response.StatusCode = 429;
                await ctx.Response.WriteAsync("Too many requests");
                return;
            }
            await next(ctx);
        }
    }
}
