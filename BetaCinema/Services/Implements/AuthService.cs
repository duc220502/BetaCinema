using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;
using MailKit;
using Microsoft.EntityFrameworkCore;
using BetaCinema.Entities;
using System.Drawing;
using Azure;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace BetaCinema.Services.Implements
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ResponseObject<DataResponseUser> _responseObject;
        private readonly ResponseObject<DataResponseToken> _responseTokenObject;
        private readonly UserConverter _converter;
        private readonly IEmailService _emailService;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
            _converter = new UserConverter();
            _responseObject = new ResponseObject<DataResponseUser>();
            _responseTokenObject = new ResponseObject<DataResponseToken>();
            _emailService = new EmailService();
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
        public DataResponseToken GenerateAccessToken(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:SecretKey").Value);

            string role = _context.Roles.FirstOrDefault(x => x.Id == user.RoleId).Code;


            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim("Email",user.Email),
                    new Claim(ClaimTypes.Role,role??"")
                }),
                Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = handler.CreateToken(tokenDescription);
            string accessToken = handler.WriteToken(token);

            string refreshToken = GenerateRefreshToken();

            RefreshToken rf = new RefreshToken
            {
                Token = refreshToken,
                ExpiredTime = DateTime.UtcNow.AddDays(1),
                UserId = user.Id
            };

            _context.RefreshTokens.Add(rf);
            _context.SaveChanges();

            DataResponseToken result = new DataResponseToken
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return result;
        }

        public ResponseObject<DataResponseToken> RenewAccessToken(Request_RenewAccessToken rq)
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
                    return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Token không hợp lệ", null);

                var token = jwtTokenHandler.ReadToken(rq.AccessToken) as JwtSecurityToken;

                if (token != null)
                {
                    long expirationTimeUnix = long.Parse(token.Payload.Exp.ToString());
                    DateTime expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationTimeUnix).UtcDateTime;
                    if (expirationDateTime > DateTime.UtcNow)
                        return _responseTokenObject.ResponseError(StatusCodes.Status401Unauthorized, "AccessToken chưa hết hạn", null);
                }

                var refreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == rq.RefreshToken);

                if (refreshToken == null)
                    return _responseTokenObject.ResponseError(StatusCodes.Status404NotFound, "RefreshToken không tồn tại trong database", null);


                if (refreshToken.ExpiredTime < DateTime.UtcNow)
                    return _responseTokenObject.ResponseError(StatusCodes.Status401Unauthorized, "RefreshToken đã hết hạn,vui lòng đăng nhập lại", null);

                var user = _context.Users.FirstOrDefault(x => x.Id == refreshToken.UserId);

                if (user == null)
                    return _responseTokenObject.ResponseError(StatusCodes.Status404NotFound, "Người dùng không tồn tại", null);

                var newToken = GenerateAccessToken(user);

                return _responseTokenObject.ResponseSuccess("Làm mới token thành công", newToken);
            }
            catch (SecurityTokenValidationException ex)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Lỗi xác thực token: " + ex.Message, null);
            }
            catch (Exception ex)
            {
                return _responseTokenObject.ResponseError(StatusCodes.Status500InternalServerError, "Lỗi không xác định: " + ex.Message, null);
            }
        }
        public ResponseObject<DataResponseConfirmEmail> ConfirmEmail(string code)
        {
            ResponseObject<DataResponseConfirmEmail> _responseCf = new ResponseObject<DataResponseConfirmEmail>();
            ConfirmEmailConverter _confirmEmailConverter = new ConfirmEmailConverter();

            var confirmEmail = _context.ConfirmEmails.FirstOrDefault(x => x.ConfirmCode == code && x.ExpiredTime > DateTime.UtcNow);

            if (confirmEmail == null)
                return _responseCf.ResponseError(StatusCodes.Status400BadRequest, "Mã xác nhận không hợp lệ hoặc đã hết hạn", null);
            

            var userCr = _context.Users.FirstOrDefault(x => x.Id == confirmEmail.UserId);

            if (userCr == null) 
                return _responseCf.ResponseError(StatusCodes.Status400BadRequest, "Người dùng không tồn tại", null);

            userCr.IsActive = true;
            _context.Users.Update(userCr);
            _context.SaveChanges();

            confirmEmail.IsConfirm = true;

            _context.ConfirmEmails.Update(confirmEmail);
            _context.SaveChanges();


           return  _responseCf.ResponseSuccess("Xác nhận email thành công", _confirmEmailConverter.EntityToDTO(confirmEmail));
            
        }
        public ResponseObject<DataResponseUser> Register(Request_Register rq)
        {
            try
            {
                if (InputHelper.checkNull(rq.UserName, rq.Name, rq.PhoneNumber, rq.Email, rq.Password))
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

                if (!InputHelper.IsValidEmail(rq.Email))
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Email không hợp lệ", null);

                if (!InputHelper.IsValidPhoneNumber(rq.PhoneNumber))
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Số điện thoại không hợp lệ", null);

                var existingUser = _context.Users.FirstOrDefault(x => x.UserName == rq.UserName || x.Email == rq.Email);
                if (existingUser != null)
                {
                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tên đăng nhập hoặc email đã tồn tại trong hệ thống", null);
                }


                var newUser = new User()
                {
                    Point = 0,
                    UserName = rq.UserName,
                    Email = rq.Email,
                    Name = rq.Name,
                    PhoneNumber = rq.PhoneNumber,
                    Password = BCryptNet.HashPassword(rq.Password),
                    IsActive = false,
                    RankCustomerId = 2,
                    UserStatusId = 2,
                    RoleId = 2
                };


                _context.Users.Add(newUser);
                _context.SaveChanges();

                Random rand = new Random();
                int randomNumber = rand.Next(1000, 10000);
                string confirmationToken = randomNumber.ToString();


                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserId = newUser.Id,
                    ConfirmCode = confirmationToken,
                    ExpiredTime = DateTime.UtcNow.AddHours(24),
                    IsConfirm = false
                };

                _context.ConfirmEmails.Add(confirmEmail);
                _context.SaveChanges();

                string subject = "Confirm Code";
                string body = "Code :" + confirmationToken;

                string emailResult = _emailService.SendEmail(newUser.Email,subject,body);

                return _responseObject.ResponseSuccess("Đăng kí thành công",_converter.EntityToDTO(newUser));
            }
            catch (Exception ex)
            {
                return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi trong quá trình xử lí:" + ex.ToString(), null);
            }
        }

        public ResponseObject<DataResponseToken> Login(Request_Login rq)
        {
            if (InputHelper.checkNull(rq.Username, rq.Password))
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền thông tin đăng nhập", null);

            var UserCr = _context.Users.FirstOrDefault(x => x.UserName == rq.Username );
            if (UserCr == null)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản  không chính xác", null);

            bool checkPass = BCryptNet.Verify(rq.Password, UserCr.Password);

            if (!checkPass)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu không chính xác", null);

            if(!UserCr.IsActive)
                return _responseTokenObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);

            UserCr.UserStatusId = 1;
            _context.Users.Update(UserCr);
            _context.SaveChanges();

            return _responseTokenObject.ResponseSuccess("Đăng nhập thành công", GenerateAccessToken(UserCr));
        }

        public IEnumerable<DataResponseUser> get_User(Pagination? pagination)
        {
            IQueryable<User> list = _context.Users.Where(x => x.IsActive);

            var dtoList = list.Select(x => _converter.EntityToDTO(x));

            if(pagination == null)
                return dtoList;

            return Result(pagination, dtoList);
        }
    }

}
