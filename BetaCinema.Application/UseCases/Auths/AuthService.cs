using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Auths
{
    public class AuthService(IUserRepository userRepository , IPasswordSecurityService passwordSecurity, 
        ITokenService tokenService , IUserStatusRepository userStatusRepository,IEmailService emailService ,
        IConfirmEmailRepository confirmEmailRepository,IConfirmationCodeService confirmationCodeService,
        ITokenGenerator tokenGenerator,IMapper mapper , IUnitOfWork unitOfWork) : IAuthService

    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordSecurityService _passwordSecurity = passwordSecurity;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserStatusRepository _userStatusRepository = userStatusRepository;

        private readonly IEmailService _emailService = emailService;
        private readonly IConfirmEmailRepository _confirmEmailRepository = confirmEmailRepository;

        private readonly ITokenGenerator _tokenGenerator = tokenGenerator;
        private readonly IMapper _mapper = mapper;

        private readonly IConfirmationCodeService _confirmationCodeService = confirmationCodeService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseObject<DataResponseToken>> Login(Request_Login rq)
        {

            var userCr = await _userRepository.GetByInformationLoginAsync(rq.UserLogin) ;

            if (userCr == null)
                throw new NotFoundException("Tài khoản không tồn tại");

            bool isPasswordValid = _passwordSecurity.VerifyPassword(rq.Password,userCr.Password);

            if (!isPasswordValid)
                throw new BadRequestException("Mật khẩu không đúng");

            if (!userCr.IsActive)
                throw new BadRequestException("Tài khoản chưa được kích hoạt");

            return ResponseObject<DataResponseToken>.ResponseSuccess("Đăng nhập thành công", await _tokenService.GenerateTokenAsync(userCr) );
        }

        public async Task<ResponseObject<object>> SendMailResetPasswordAsync(string account, ConfirmationMethod method)
        {
            var userCr = await _userRepository.GetByEmailOrNumberPhoneAsync(account)
            ?? throw new NotFoundException("Tài khoản không tồn tại");
            
            var (confirmEmail,strategy) =  _confirmationCodeService.CreateCode(userCr.Id,method,CodePurpose.PasswordReset);

            var emailBody = strategy.CreateEmailBody(confirmEmail.ConfirmCode, userCr.Email);
            await _emailService.SendEmailAsync(userCr.Email, "Yêu cầu đặt lại mật khẩu", emailBody);

            return ResponseObject<object>.ResponseSuccess("Gửi mail reset password thành công",null);
        }

        public async Task<ResponseObject<string>> VerifyResetCodeAsync(Request_VerifyCode rq)
        {
            var user = await _userRepository.GetByIdAsync(rq.userId)
            ?? throw new NotFoundException("Tài khoản không tồn tại");

            var confirmationCode = await _confirmEmailRepository.GetByUserIdAndCodeAsync(user.Id,rq.Code)
                ?? throw new NotFoundException("Mã xác thực không tìm thấy");

            if (confirmationCode.ExpiredTime < DateTime.UtcNow || confirmationCode.Purpose != CodePurpose.PasswordReset)
            {
                throw new BadRequestException("Mã xác thực không hợp lệ hoặc đã hết hạn.");
            }

            confirmationCode.IsConfirm = true;
            _confirmEmailRepository.Update(confirmationCode);
            await _unitOfWork.SaveChangesAsync();

            var temporaryToken = _tokenGenerator.GeneratePasswordResetToken(user.Id);
            return ResponseObject<string>.ResponseSuccess("Xác thực mã thành công",temporaryToken);
        }

        public async Task<ResponseObject<DataResponseUser>> ResetPasswordAsync(Guid userId,Request_ResetPassword rq)
        {
            var userCr = await _userRepository.GetByIdWithDetailsAsync(userId);

            if (_passwordSecurity.VerifyPassword(rq.NewPassword, userCr!.Password))
                throw new ConflictException("Mật khẩu mới không được trùng với mật khẩu cũ.");

            userCr.Password = _passwordSecurity.HashPassword(rq.NewPassword);
            _userRepository.Update(userCr);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUser>.ResponseSuccess("Đổi mật khẩu thành công", _mapper.Map<DataResponseUser>(userCr));
        }
    }
}
