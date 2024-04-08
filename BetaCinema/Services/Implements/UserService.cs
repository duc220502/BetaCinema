using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.Payloads.Convertes;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;
using BetaCinema.Services.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace BetaCinema.Services.Implements
{
    public class UserService : BaseService, IUserService
    {
        private readonly ResponseObject<DataResponseUser> _responseObject;
        private readonly UserConverter _converter;
        private readonly IEmailService _emailService;
        public UserService()
        {
            _converter = new UserConverter();
            _responseObject = new ResponseObject<DataResponseUser>();
            _emailService = new EmailService();
        }

        public ResponseObject<DataResponseUser> ChangPassword(int id, string oldPassword, string newPassword)
        {
            if (InputHelper.checkNull(oldPassword, newPassword))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Vui lòng điền đầy đủ thông tin", null);

            var userCr = _context.Users.FirstOrDefault(x => x.Id == id);

            if (!userCr.IsActive)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Tài khoản chưa được kích hoạt", null);

            if (!BCryptNet.Verify(oldPassword,userCr.Password))
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Mật khẩu cũ không chính xác", null);

            userCr.Password = BCryptNet.HashPassword(newPassword);

            _context.Users.Update(userCr);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Đổi mật khẩu thành công", _converter.EntityToDTO(userCr));
        }

        public ResponseObject<DataResponseUser> ForgotPassword(int id)
        {
            try
            {
                Random rand = new Random();
                int randomNumber = rand.Next(1000, 10000);
                string confirmationToken = randomNumber.ToString();

                var userCr = _context.Users.FirstOrDefault(x=>x.Id == id);

                if (userCr == null)

                    return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "User không tồn tại", null);

                ConfirmEmail confirmEmail = new ConfirmEmail
                {
                    UserId = id,
                    ConfirmCode = confirmationToken,
                    ExpiredTime = DateTime.UtcNow.AddHours(24),
                    IsConfirm = false
                };

                _context.ConfirmEmails.Add(confirmEmail);
                _context.SaveChanges();

                string subject = "Confirm Code";
                string body = "Code :" + confirmationToken;

                string emailResult = _emailService.SendEmail(userCr.Email, subject, body);

                return _responseObject.ResponseSuccess("Đăng kí thành công", _converter.EntityToDTO(userCr));
            }
            catch (Exception ex)
            {
                return _responseObject.ResponseError(StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi trong quá trình xử lí:" + ex.ToString(), null);
            }
        }

        public ResponseObject<DataResponseUser> NewPassword(string code, string newPass)
        {
            var confirmEmail = _context.ConfirmEmails.FirstOrDefault(x => x.ConfirmCode == code && x.ExpiredTime > DateTime.UtcNow);

            if (confirmEmail == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Mã xác nhận không hợp lệ hoặc đã hết hạn", null);

            var userCr = _context.Users.FirstOrDefault(x => x.Id == confirmEmail.UserId);

            if (userCr == null)
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Người dùng không tồn tại", null);

            userCr.Password = BCryptNet.HashPassword(newPass);
            _context.Users.Update(userCr);
            _context.SaveChanges();

            confirmEmail.IsConfirm = true;

            _context.ConfirmEmails.Update(confirmEmail);
            _context.SaveChanges();

            return _responseObject.ResponseSuccess("Đổi mật khẩu mới thành công",_converter.EntityToDTO(userCr));

        }
    }
}
