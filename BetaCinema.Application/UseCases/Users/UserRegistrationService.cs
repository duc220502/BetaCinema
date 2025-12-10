using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Enums;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Users
{
    public class UserRegistrationService(IUserRepository userRepository, IMapper mapper,
        IRankCustomerRepository rankCustomerRepository, IRoleRepository roleRepository,
        IUserStatusRepository userStatusRepository, IPasswordSecurityService passwordSecurity,
        IConfirmationCodeService confirmationCodeService, IEmailService emailService,
        IConfirmEmailRepository confirmEmailRepository, IUnitOfWork unitOfWork) : IUserRegistrationService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRankCustomerRepository _rankCustomerRepository = rankCustomerRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly IUserStatusRepository _userStatusRepository = userStatusRepository;
        private readonly IPasswordSecurityService _passwordSecurity = passwordSecurity;
        private readonly IConfirmationCodeService _confirmationCodeService = confirmationCodeService;
        private readonly IEmailService _emailService = emailService;
        private readonly IConfirmEmailRepository _confirmEmailRepository = confirmEmailRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseObject<DataResponseUser>> ConfirmEmailRegister(Request_ConfirmEmailRegister rq)
        {
            var confirmEmailCr = await _confirmEmailRepository.GetByUserIdAndCodeAsync(rq.UserId, rq.Code) 
                ?? throw new NotFoundException("Không có ConfirmEmail hợp lệ");

            var userCr = await _userRepository.GetByIdWithDetailsAsync(rq.UserId)
                ?? throw new NotFoundException("Không tìm thấy user");



            if (confirmEmailCr.ExpiredTime < DateTime.UtcNow || confirmEmailCr.Purpose != CodePurpose.EmailConfirmation)
                throw new BadRequestException("Mã xác thực không hợp lệ hoặc đã hết hạn.");

            confirmEmailCr.IsConfirm = true;

            userCr.UserStatusId = (int)Domain.Enums.UserStatus.Active;

            _userRepository.Update(userCr);
            _confirmEmailRepository.Update(confirmEmailCr);

            await _unitOfWork.SaveChangesAsync();


            return ResponseObject<DataResponseUser>.ResponseSuccess("Xác thực kích hoạt tài khoản thành công", _mapper.Map<DataResponseUser>(userCr));
        }

        public async Task<ResponseObject<DataResponseUser>> Register(Request_Register rq, ConfirmationMethod method)
        {

            var hashedPassword = _passwordSecurity.HashPassword(rq.Password);

            var defaultRank = (int) UserRank.Standard;

            var defaultRole = (int) UserRole.Member;

            var defaultUserStatus = (int) Domain.Enums.UserStatus.PendingActivation;

            var newUser = new User()
            {

                UserName = rq.UserName,
                FullName = rq.FullName,
                Email = rq.Email,
                NumberPhone = rq.NumberPhone,
                Point = 0,
                Password = hashedPassword,
                RankCustomerId = defaultRank,
                RoleId = defaultRole,
                UserStatusId = defaultUserStatus

            };

            _userRepository.Add(newUser);

            var (confirmEmail, strategy) = _confirmationCodeService.CreateCode(newUser.Id, method, CodePurpose.EmailConfirmation);

            _confirmEmailRepository.Add(confirmEmail);

            await _unitOfWork.SaveChangesAsync();


            var createdUser = await _userRepository.GetByIdWithDetailsAsync(newUser.Id)
                ?? throw new InvalidOperationException("Không thể tải lại thông tin người dùng vừa tạo.");
            
            var userDto = _mapper.Map<DataResponseUser>(createdUser);

            var emailBody = strategy.CreateEmailBody(confirmEmail.ConfirmCode, newUser.Email);
            await _emailService.SendEmailAsync(newUser.Email, "Yêu cầu xác thực đăng kí tài khoản", emailBody);

            return ResponseObject<DataResponseUser>.ResponseSuccess("Đăng kí thành công vui lòng xác thực để kích hoạt tài khoản", userDto);

        }
    }
}
