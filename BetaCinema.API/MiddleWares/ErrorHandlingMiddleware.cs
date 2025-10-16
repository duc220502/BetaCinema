using BetaCinema.Application.Common;
using BetaCinema.Application.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Security;

namespace BetaCinema.API.MiddleWares
{
    public class ErrorHandlingMiddleware(RequestDelegate next , ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/oauth2/callback")
             || context.Request.Path.StartsWithSegments("/signin-oidc"))
            {
                await _next(context);
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = StatusCodes.Status500InternalServerError; 
            var message = "An internal server error has occurred.";

            switch (exception)
            {
                case NotFoundException ex:
                    statusCode = StatusCodes.Status404NotFound;
                    message = ex.Message;
                    break;
                case ConflictException ex:
                    statusCode = StatusCodes.Status409Conflict;
                    message = ex.Message;
                    break;
                case ForbiddenException ex:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = ex.Message;
                    break;
                case BadRequestException ex:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;
                case SecurityTokenValidationException ex:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;

                case AccessTokenNotExpiredException ex:
                    statusCode = StatusCodes.Status403Forbidden;
                    message = ex.Message;
                    break;

                case InvalidOperationException ex:
                    statusCode = StatusCodes.Status404NotFound;
                    message = ex.Message;
                    break;

                case DataConflictException ex:
                    statusCode = StatusCodes.Status409Conflict;
                    message = ex.Message;
                    break;

                case SecurityException ex:
                    statusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;
                case FileNotFoundException ex:
                    statusCode = StatusCodes.Status404NotFound;
                    message = ex.Message;
                    break;

            }

            context.Response.StatusCode = statusCode;

            var errorResponse = ResponseObject<object>.ResponseError( message, null);

            return context.Response.WriteAsJsonAsync(errorResponse);
        }
    }
}
