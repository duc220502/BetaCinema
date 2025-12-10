using BetaCinema.Application.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
namespace BetaCinema.API.Extensions
{
    public static class ApiBehaviorExtensions
    {
        public static IServiceCollection AddCustomApiBehavior(this IServiceCollection services)
        {
             services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // PascalCase → camelCase
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                })
                .ConfigureApiBehaviorOptions(options =>
                {
                    // Unify validation error response
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(x => x.Value?.Errors.Count > 0)
                            .ToDictionary(
                                e => e.Key,
                                e => e.Value!.Errors.Select(err => err.ErrorMessage).ToArray()
                            );

                        var response = ResponseObject<Dictionary<string, string[]>>.ResponseError(
                            "Dữ liệu không hợp lệ",
                            errors
                        );

                        return new BadRequestObjectResult(response);
                    };
                });

            return services;
        }
    }
}
