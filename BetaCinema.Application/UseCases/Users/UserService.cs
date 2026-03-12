using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Auth.External;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Catching;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Shared.Pagination;
using FluentValidation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Users
{
    public class UserService(IUserRepository userRepository  ,
        IMapper mapper,IPasswordSecurityService passwordSecurity ,
        ICurrentUserservice currentUser,IUnitOfWork unitOfWork , 
        IUserStatusRepository userStatusRepository ,
        IOptions<LoyaltySettings> loyaltySettings , 
        IRankCustomerRepository rankCustomerRepository,
        IBackgroundJobService backgroundJobService  ,
        IExternalLoginRepository externalLoginRepository,
        IExternalLinkingStore externalLinkingStore,
        IOtpService otpService,
        IEmailService emailService) : IUserService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserStatusRepository _userStatusRepository = userStatusRepository;
        private readonly ICurrentUserservice _currentUser = currentUser;
        private readonly IPasswordSecurityService _passwordSecurity = passwordSecurity;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly LoyaltySettings _loyaltySettings = loyaltySettings.Value;
        private readonly IRankCustomerRepository _rankCustomerRepository = rankCustomerRepository;
        private readonly IBackgroundJobService _backgroundJobService = backgroundJobService;
        private readonly IExternalLoginRepository _externalLoginRepository = externalLoginRepository;
        private readonly IExternalLinkingStore _externalLinkingStore = externalLinkingStore;
        private readonly IOtpService _otpService = otpService;
        private readonly IEmailService _emailService = emailService;

        /*private async Task<User> GetCurrentUserAndValidateAsync()
        {
            var userId = _currentUser.UserId;
            var user = await _userRepository.GetByIdWithDetailsAsync(userId);

            if (user == null)
                throw new NotFoundException("User không tồn tại");

            if (!user.IsActive)
                throw new BadRequestException("Tài khoản chưa được kích hoạt");

            return user;
        }*/
        public async Task<ResponseObject<DataResponseUser>> ChangePasswordAsync(Request_ChangePassword rq)
        {
           

            var userCr = await GetAndValidateCurrentUserAsync();

            bool isPasswordValid = _passwordSecurity.VerifyPassword(rq.OldPass, userCr.Password);

            if (!isPasswordValid)

                throw new BadRequestAppException("Mật khẩu không chính xác");

            if (rq.OldPass == rq.NewPass)

                throw new ConflictAppException("Trùng mật khẩu cũ");


            var hashedPassword =_passwordSecurity.HashPassword(rq.NewPass);

            userCr.Password = hashedPassword;

            
            _userRepository.Update(userCr);

            var dto = _mapper.Map<DataResponseUser>(userCr);
            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUser>.ResponseSuccess("Thay đổi mật khẩu thành công",dto);
        }

        public async Task<ResponseObject<DataResponseUser>> GetUserById(Guid id)
        {
            var currentUserId = _currentUser.UserId;
            var currentUserRole = _currentUser.Role;

            if (currentUserRole != nameof(UserRole.Admin) && currentUserId != id)
            {
                throw new ForbiddenAppException("Bạn không có quyền xem thông tin của người dùng này.");
            }

            var userCr = await _userRepository.GetByIdWithDetailsAsync(id) ?? throw new NotFoundAppException("User không tồn tại");

            return ResponseObject<DataResponseUser>.ResponseSuccess("Lấy thông tin user thành công", _mapper.Map<DataResponseUser>(userCr));
        }

        public async Task<ResponseObject<IEnumerable<DataResponseUser>>> GetUsers(Pagination pagination)
        {
            var pageResult = await _userRepository.GetAllUsersAsync(pagination);

            var results = pageResult.Data.Select(x=>_mapper.Map<DataResponseUser>(x));

              return ResponseObject<IEnumerable<DataResponseUser>>.ResponseSuccess("Lấy danh sách user thành công", results);

        }


        public async Task<ResponseObject<DataResponseUser>> UpdateMyProfile(Request_UpdateMyProfile rq)
        {
            var userCr = await GetAndValidateCurrentUserAsync();

            _mapper.Map(rq, userCr);

            _userRepository.Update(userCr);

            var dto = _mapper.Map<DataResponseUser>(userCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUser>.ResponseSuccess("Cập nhật thông tin thành công", dto);

        }

        public async Task<ResponseObject<DataResponseUser>> UpdateUserByAdmin(Guid id , Request_UpdateUserByAdmin rq)
        {

            var userCr = await _userRepository.GetByIdWithDetailsAsync(id)
                ?? throw new NotFoundAppException($"Không tìm thấy người dùng với ID: {id}");

            _mapper.Map(rq, userCr);

            _userRepository.Update(userCr);

            var dto = _mapper.Map<DataResponseUser>(userCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUser>.ResponseSuccess("Cập nhật thông tin thành công", dto);

        }

        public async Task<ResponseObject<DataResponseUser>> DeleteUser(Guid id)
        {
            var userCr = await _userRepository.GetByIdWithDetailsAsync(id)
             ?? throw new NotFoundAppException("User không tồn tại.");

            userCr.UserStatusId = (int)Domain.Enums.UserStatus.Banned;

            _userRepository.Update(userCr);

            var dto = _mapper.Map<DataResponseUser>(userCr);
            await _unitOfWork.SaveChangesAsync();
            return ResponseObject<DataResponseUser>.ResponseSuccess("Xóa User  thành công", dto);

        }

        public async Task UpdateUserRankAfterPurchaseAsync(User user, decimal amountSpent)
        {
            var pointConversionRate = _loyaltySettings.PointConversionRate;

            if (pointConversionRate > 0)
            {
                int pointsEarned = (int)Math.Floor(amountSpent / pointConversionRate);
                user.Point += pointsEarned;
                
            }

            var allRanks = await _rankCustomerRepository.GetAllRankCustomersDesAsync();

            var newRank = allRanks?.FirstOrDefault(r => user.Point >= r.MinimumPoint);

            if (newRank != null && user.RankCustomerId != newRank.Id)
            {
               
                user.RankCustomerId = newRank.Id;

                _backgroundJobService.Enqueue<IRankUpNotificationService>(
                 emailService => emailService.SendRankUpAsync(user.Email,user.UserName??"User", newRank.RankName));

            }

        }

        public async Task<User> GetAndValidateCurrentUserAsync()
        {
            var userId = _currentUser.GetRequiredUserId();

            var user = await _userRepository.GetByIdWithDetailsAsync(userId!);
            if (user == null)
                throw new NotFoundAppException($"User với ID '{userId}' trong token không tồn tại trong hệ thống.");

            if (!user.IsActive)
                throw new BadRequestAppException("Tài khoản của bạn đã bị khóa hoặc chưa được kích hoạt.");

            return user;
        }

        public async Task<ExternalAuthResult> FindOrCreateExternalUserAsync(string provider, string providerKey, string email, string? name, CancellationToken ct = default)
        {
            var existingLink = await _externalLoginRepository.GetExistingLink(provider,providerKey,ct);

            if (existingLink != null)
            {
                var u = await _userRepository.GetByIdWithRoleAsync(existingLink.UserId, ct)
                        ?? throw new NotFoundAppException("Không có user");
                return new ExternalAuthResult() { RequiresLinking = false ,LinkingToken = null , Email = u.Email , Provider = provider , User = u };
            }

            var userByEmail = await _userRepository.GetByEmailOrNumberPhoneAsync(email, ct);

            if (userByEmail != null)
            {
                var linkingToken = Guid.NewGuid().ToString("N");

                await _externalLinkingStore.SaveStateAsync(linkingToken, new ExternalLinkState
                {
                    Provider = provider,
                    ProviderKey = providerKey,
                    Email = email,
                    UserId = userByEmail.Id,
                }, ttl: TimeSpan.FromMinutes(10), ct);

                var otp = _otpService.GenerateNumericCode(6);
                await _externalLinkingStore.SaveOtpAsync(linkingToken, otp, ttl: TimeSpan.FromMinutes(5), ct);

                var subject = "OTP xác thực liên kết đăng nhập";
                var body = $"Mã xác thực của bạn là : {otp}";
                await _emailService.SendEmailAsync(email, subject, body);

                return new ExternalAuthResult() { RequiresLinking = true,LinkingToken = linkingToken ,Email = email , Provider = provider ,User = null};
            }

            var user = new User
            {
                Email = email,
                UserName = name ?? email.Split('@')[0],
                FullName = name ?? " User",
                NumberPhone = "",
                Password = "",
                Point = 0,
                RankCustomerId = (int)UserRank.Standard,
                RoleId = (int)UserRole.Member,
                UserStatusId = (int)Domain.Enums.UserStatus.Active
            };


            var ExternalLogin = new ExternalLogin
            {
                Provider = provider,
                ProviderKey = providerKey,
                User = user
            };
            _userRepository.Add(user);
            _externalLoginRepository.Add(ExternalLogin);

            await _unitOfWork.SaveChangesAsync(ct);

            var created = await _userRepository.GetByIdWithRoleAsync(user.Id, ct)
                      ?? throw new NotFoundAppException("Không có user");


            return new ExternalAuthResult() { RequiresLinking = false , LinkingToken = null , Email = created.Email , Provider =  provider , User = created };

        }

        public async Task<ResponseObject<DataResponseUser>> GetMyProfile()
        {
            var userCr = await GetAndValidateCurrentUserAsync();

            var dto = _mapper.Map<DataResponseUser>(userCr);

            return ResponseObject<DataResponseUser>.ResponseSuccess("Lấy thông tin user thành công", dto);


        }
    }
}
