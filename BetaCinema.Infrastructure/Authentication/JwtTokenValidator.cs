using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces.Auths;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Authentication
{
    public class JwtTokenValidator (IOptions<JWTOptions> options) : ITokenValidator
    {
        private readonly JWTOptions _options = options.Value;
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {

            var secretKey = _options.SecretKey;
            TokenValidationParameters _tokenValidationParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {

                var validationParamsWithLifetime = _tokenValidationParams.Clone();

                validationParamsWithLifetime.ValidateLifetime = true;
                var parsedToken = tokenHandler.ReadJwtToken(token);
                tokenHandler.ValidateToken(token, validationParamsWithLifetime, out _);

                throw new AccessTokenNotExpiredException("Access Token chưa hết hạn.");
            }
            
        
            catch (SecurityTokenExpiredException)
            {
                var validationParamsWithoutLifetime = _tokenValidationParams.Clone();
                validationParamsWithoutLifetime.ValidateLifetime = false;

                var principal = tokenHandler.ValidateToken(token, validationParamsWithoutLifetime, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new BadRequestException("Thuật toán của Access Token không hợp lệ.");
                }

                return principal;
            }
            catch (AccessTokenNotExpiredException)
            {
                throw;
            }
            catch
            {
                throw new BadRequestException("Access Token không hợp lệ.");
            }
            
        }

           
        
    }
}
