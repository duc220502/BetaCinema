using BetaCinema.Application.Common;
using BetaCinema.Application.Exceptions;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security;

namespace BetaCinema.API.MiddleWares
{
    public class ErrorHandlingMiddleware(RequestDelegate next , ILogger<ErrorHandlingMiddleware> logger , IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;
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
            catch (AppException aex)
            {
                var status = MapExceptionToStatus(aex);
                await WriteProblem(context, status, aex.ErrorCode, aex.Message, aex.Meta, aex);
            }
            catch (ValidationException vex)
            {
                var errors = vex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                await WriteProblem(context, StatusCodes.Status422UnprocessableEntity,
                    "VALIDATION_ERROR", "Dữ liệu không hợp lệ.", new { errors }, vex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                await WriteProblem(context, StatusCodes.Status500InternalServerError,
               "INTERNAL_ERROR",
               _env.IsDevelopment() ? ex.Message : "Đã xảy ra lỗi hệ thống.",
               _env.IsDevelopment() ? new { exception = ex.GetType().Name } : null,
               ex);
            }
        }
        /*private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            *//* context.Response.ContentType = "application/json";
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


            /*if (context.Response.HasStarted)  return Task.CompletedTask;

            context.Response.ContentType = "application/problem+json";
            var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
            var (status, message) = MapExceptionToStatus(exception);

            context.Response.StatusCode = status;

            var errorResponse = ResponseObject<object>.ResponseError(message+traceId, null);

            return context.Response.WriteAsJsonAsync(errorResponse);*//*

        }*/


        private static int MapExceptionToStatus(Exception ex) => ex switch
        {
            NotFoundAppException => StatusCodes.Status404NotFound,
            ConflictAppException => StatusCodes.Status409Conflict,
            ForbiddenAppException => StatusCodes.Status403Forbidden,
            NeedLinkingException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status400BadRequest
        };

        private async Task WriteProblem(
        HttpContext ctx, int status, string code, string message, object? meta, Exception ex)
        {
            if (ctx.Response.HasStarted) return;

            _logger.LogError(ex, "[{Code}] {Message} at {Path}", code, message, ctx.Request.Path);

            ctx.Response.StatusCode = status;
            ctx.Response.ContentType = "application/problem+json";

            var traceId = Activity.Current?.Id ?? ctx.TraceIdentifier;

            var payload = new
            {
                error = new { code, message, meta },
                traceId
            };

            await ctx.Response.WriteAsJsonAsync(payload);
        }
    }

}
