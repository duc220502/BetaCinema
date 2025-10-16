using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Domain.Contants;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Auths
{
    public class TokenService(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, ITokenGenerator tokenGenerator,ITokenValidator tokenValidator , IUnitOfWork unitOfWork) : ITokenService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
        private readonly ITokenValidator _tokenValidator = tokenValidator;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        private string GenerateRefreshToken()
        {
            var random = new Byte[32];

            using (var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }
        public async Task<DataResponseToken> GenerateTokenAsync(User user)
        {
            string role = user.Role?.RoleName ?? "không có role";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(AppClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };

            string accessToken = _tokenGenerator.GenerateAccessToken(user.Id, 10, claims);

            string refreshToken = GenerateRefreshToken();

            var rfToken = await _refreshTokenRepository.GetByUserIdAsync(user.Id);
            if (rfToken == null)
            {
                RefreshToken rf = new RefreshToken
                {
                    Token = refreshToken,
                    ExpiredTime = DateTime.UtcNow.AddDays(1),
                    UserId = user.Id
                };

                _refreshTokenRepository.Add(rf);

            }
            else
            {
                rfToken.Token = refreshToken;
                rfToken.ExpiredTime = DateTime.UtcNow.AddDays(1);
                _refreshTokenRepository.Update(rfToken);

            }
            await _unitOfWork.SaveChangesAsync();

            DataResponseToken result = new DataResponseToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return result;
        }

        public async Task<ResponseObject<DataResponseToken>> RenewToken(Request_RenewToken rq)
        { 
            var principal = _tokenValidator.GetPrincipalFromExpiredToken(rq.AccessToken);

            var userIdFromTokenString = principal!.FindFirst(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdFromTokenString?.Value, out var userIdFromToken))
                throw new BadRequestException("Access Token không chứa User ID hợp lệ.");


            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(rq.RefreshToken);

            if (refreshToken == null)
                throw new NotFoundException("RefreshToken không tồn tại trong database");

            if (refreshToken.UserId != userIdFromToken)
                throw new ForbiddenException("Hành động không được phép: Refresh token không khớp với người dùng.");

            if (refreshToken.ExpiredTime < DateTime.UtcNow)
                throw new BadRequestException("RefreshToken đã hết hạn,vui lòng đăng nhập lại");

            var user = await _userRepository.GetByIdWithDetailsAsync(refreshToken.UserId);

            if (user == null)
                throw new NotFoundException("Người dùng không tồn tại");

            var newToken = await GenerateTokenAsync(user);

            return ResponseObject<DataResponseToken>.ResponseSuccess("Làm mới token thành công", newToken);
            
        }
    }


}
