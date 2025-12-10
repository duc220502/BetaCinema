using BetaCinema.Application.Common;
using BetaCinema.Application.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security;

namespace BetaCinema.API.MiddleWares
{
    public class ErrorHandlingMiddleware(RequestDelegate next , ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            /*if (context.Request.Path.StartsWithSegments("/oauth2/callback")
            || context.Request.Path.StartsWithSegments("/signin-oidc"))
            {
                await _next(context);
                return;
            }*/

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
            /* context.Response.ContentType = "application/json";
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

             return context.Response.WriteAsJsonAsync(errorResponse);*/


            if (context.Response.HasStarted)  return Task.CompletedTask;

            context.Response.ContentType = "application/problem+json";
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            var (status, message) = MapExceptionToStatus(exception);

            context.Response.StatusCode = status;

            var errorResponse = ResponseObject<object>.ResponseError(message+traceId, null);

            return context.Response.WriteAsJsonAsync(errorResponse);

        }


        private static (int status, string message) MapExceptionToStatus(Exception ex) => ex switch
        {
            NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
            ConflictException => (StatusCodes.Status409Conflict, ex.Message),
            DataConflictException => (StatusCodes.Status409Conflict, ex.Message),
            ForbiddenException => (StatusCodes.Status403Forbidden, ex.Message),
            BadRequestException => (StatusCodes.Status400BadRequest, ex.Message),
            SecurityTokenValidationException => (StatusCodes.Status401Unauthorized, ex.Message),
            SecurityException => (StatusCodes.Status401Unauthorized, ex.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, ex.Message),
            OperationCanceledException => (499, ex.Message),
            BadHttpRequestException => (StatusCodes.Status400BadRequest, ex.Message),
            FileNotFoundException => (StatusCodes.Status404NotFound, ex.Message),
            InvalidOperationException => (StatusCodes.Status400BadRequest, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, ex.Message)
        };
    }

}
