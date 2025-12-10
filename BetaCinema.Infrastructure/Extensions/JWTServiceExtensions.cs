using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Domain.Interfaces;
using BetaCinema.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class JWTServiceExtensions
    {
        public static AuthenticationBuilder AddJWTAuthentication(this AuthenticationBuilder authBuilder, IConfiguration config)
        {
            var jwtSection = config.GetSection("JwtOptions");
            authBuilder.Services.Configure<JWTOptions>(jwtSection);
            var jwtOptions = jwtSection.Get<JWTOptions>();
            var key = Encoding.UTF8.GetBytes(jwtOptions?.SecretKey ?? "");

            authBuilder

              .AddJwtBearer("Bearer", opt =>
              {
                  opt.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidIssuer = jwtOptions?.Issuer,
                      ValidateAudience = true,
                      ValidAudience = jwtOptions?.Audience,
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = new SymmetricSecurityKey(key),
                      ValidateLifetime = true,
                      ClockSkew = TimeSpan.Zero
                  };
              });

            authBuilder.Services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            authBuilder.Services.AddScoped<ITokenValidator, JwtTokenValidator>();
            return authBuilder;
        }
    }
}
