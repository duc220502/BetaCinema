using BetaCinema.API.MiddleWares;

namespace BetaCinema.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            // app.UseMiddleware<RateLimitMiddleware>();

            app.UseCors("AllowFrontend");

            return app;
        }
    }
}
