using BetaCinema.Domain.Enums;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace BetaCinema.API.Extensions
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
               .AddAuthenticationSchemes("Bearer")
               .RequireAuthenticatedUser()
               .Build();

                options.AddPolicy("PasswordReset", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("purpose",CodePurpose.PasswordReset.ToString() );
                });

                options.AddPolicy("BffPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // (tuỳ chọn) policy.RequireClaim("amr", "pwd", "external");
                });
            });

            return services;
        }
    }
}
