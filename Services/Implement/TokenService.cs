using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetaCinema.Services.Implement
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(int userId, int expireTime, List<Claim> claims)
        {
            var handler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["AppSettings:SecretKey"]);


            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expireTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);

        }

        public string GenerateResetToken(int id)
        {
            return GenerateJwtToken(id, 5, new List<Claim>());
        }
    }
}
