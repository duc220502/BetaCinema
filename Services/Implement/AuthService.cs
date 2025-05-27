using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BetaCinema.Services.Implement
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly ResponseObject<DataResponseUser> _responseObject;
        private readonly ResponseObject<DataResponseToken> _responseObjectToken;
        private readonly UserConverter _userConverter;
        private readonly IEmailService _IEmailService;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthService(IEmailService iEmailService, IConfiguration configuration)
        {
            _responseObject = new ResponseObject<DataResponseUser>();
            _responseObjectToken = new ResponseObject<DataResponseToken>();
            _userConverter = new UserConverter();
            _IEmailService = iEmailService;
            _configuration = configuration;
            _tokenService = new TokenService(configuration);
        }

        public async Task<ResponseObject<DataResponseToken>> Login(Request_Login rq)
        {
            if (InputHelper.checkNull(new string[] { rq.UserLogin, rq.Password }))
                return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var userCr =await _context.Users.FirstOrDefaultAsync(x => x.UserName == rq.UserLogin || x.Email == rq.UserLogin || x.NumberPhone == rq.UserLogin);

            if (userCr == null)
                return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản không tồn tại", null);

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(rq.Password, userCr.Password);

            if (!isPasswordValid)
                return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không đúng", null);

            if (!userCr.IsActive)
                return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);

            userCr.UserStatusId = 1;

            _context.Users.Update(userCr);
            await _context.SaveChangesAsync();

            return _responseObjectToken.ResponseSuccess("Đăng nhập thành công",await GenerateAccessToken(userCr));
        }

        public async Task<ResponseObject<DataResponseUser>> Register(Request_Register rq)
        {
            if (rq == null || InputHelper.checkNull(new string[] { rq.UserName, rq.FullName, rq.Email, rq.NumberPhone, rq.Password }))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            if (!InputHelper.IsValidEmail(rq.Email))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email không hợp lệ", null);

            if (!InputHelper.IsValidPhoneNumber(rq.NumberPhone))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại không hợp lệ", null);

            var checkUser = await _context.Users.AnyAsync(x=>x.Email == rq.Email || x.UserName == rq.UserName || x.NumberPhone == rq.NumberPhone);

            if (checkUser)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Trùng thông tin đã được đki", null);

            var code = GennerateCode("D");

            var emailMessage = new EmailMessage()
            {
                To = new List<MailboxAddress> { new MailboxAddress(rq.UserName, rq.Email) },
                Subject = "Xác nhận email",
                Content = "Mã xác nhận :" + code

            };

            var checkSendMail = (await _IEmailService?.SendMail(emailMessage)).status == StatusCodes.Status200OK ? true : false;

            if (checkSendMail)
            {

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(rq.Password);
                var newUser = new User()
                {
                    
                    UserName = rq.UserName,
                    FullName = rq.FullName,
                    Email = rq.Email,
                    NumberPhone = rq.NumberPhone,
                    Point = 0,
                    Password = hashedPassword,
                    IsActive = false,
                    RankCustomerId = 1,
                    RoleId = 1,
                    UserStatusId = 1

                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var newConfirmEmail = new ConfirmEmail()
                {
                    ConfirmCode = code,
                    ExpiredTime = DateTime.UtcNow.AddHours(1),
                    IsConfirm = false,
                    UserId = newUser.Id,

                };
                _context.ConfirmEmails.Add(newConfirmEmail);
                await _context.SaveChangesAsync();

                return _responseObject.ResponseSuccess("Đăng kí thành công", _userConverter.EntityToDTO(newUser));
            }



            return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Đăng kí thất bại", null);


        }

        private string GenerateRefreshToken()
        {
            var random = new Byte[32];

            using (var item = RandomNumberGenerator.Create())
            {
                item.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        
        public async Task<DataResponseToken> GenerateAccessToken(User user)
        {
            string role = (await _context.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId))?.Code ?? "";

            var claims = new List<Claim>
            {
                new Claim("Email", user.Email),
                new Claim(ClaimTypes.Role, role)
            };


            string accessToken = _tokenService.GenerateJwtToken(user.Id,2,claims);

            string refreshToken = GenerateRefreshToken();

            var rfToken = await _context.RefreshTokens.FirstOrDefaultAsync(x=>x.UserId == user.Id);
            if(rfToken == null)
            {
                RefreshToken rf = new RefreshToken
                {
                    Token = refreshToken,
                    ExpiredTime = DateTime.UtcNow.AddDays(1),
                    UserId = user.Id
                };

                _context.RefreshTokens.Add(rf);
                await _context.SaveChangesAsync();
            }else
            {
                rfToken.Token = refreshToken;
                rfToken.ExpiredTime = DateTime.UtcNow.AddDays(1);
                _context.RefreshTokens.Update(rfToken);
                await _context.SaveChangesAsync();
            }
            

            DataResponseToken result = new DataResponseToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return result;
        }

       

        public async Task<ResponseObject<DataResponseToken>> RenewToken(Request_RenewAccessToken rq)
        {
            try
            {
                JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
                var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

                var tokenValidation = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)

                };

                var tokenAuthentication = jwtTokenHandler.ValidateToken(rq.AccessToken, tokenValidation, out var validatedToken);

                if (!(validatedToken is JwtSecurityToken jwtSecurityToken) || jwtSecurityToken.Header.Alg != SecurityAlgorithms.HmacSha256)
                    return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);

                var token = jwtTokenHandler.ReadToken(rq.AccessToken) as JwtSecurityToken;

                if (token != null)
                {
                    long expirationTimeUnix = long.Parse(token.Payload.Exp.ToString());
                    DateTime expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationTimeUnix).UtcDateTime;
                    if (expirationDateTime > DateTime.UtcNow)
                        return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "AccessToken chưa hết hạn", null);
                }

                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == rq.RefreshToken);

                if (refreshToken == null)
                    return _responseObjectToken.ResponseError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);


                if (refreshToken.ExpiredTime < DateTime.UtcNow)
                    return _responseObjectToken.ResponseError(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn,vui lòng đăng nhập lại", null);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == refreshToken.UserId);

                if (user == null)
                    return _responseObjectToken.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);

                var newToken = await GenerateAccessToken(user);

                return _responseObjectToken.ResponseSuccess("Làm mới token thành công", newToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                return _responseObjectToken.ResponseError(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
            }
            catch (Exception ex)
            {
                return _responseObjectToken.ResponseError(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
            }
        }
    }
}
