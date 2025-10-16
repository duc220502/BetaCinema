using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
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
        IExternalLoginRepository externalLoginRepository) : IUserService
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

                throw new BadRequestException("Mật khẩu không chính xác");

            if (rq.OldPass == rq.NewPass)

                throw new ConflictException("Trùng mật khẩu cũ");


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
                throw new ForbiddenException("Bạn không có quyền xem thông tin của người dùng này.");
            }

            var userCr = await _userRepository.GetByIdWithDetailsAsync(id) ?? throw new NotFoundException("User không tồn tại");

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
                ?? throw new NotFoundException($"Không tìm thấy người dùng với ID: {id}");

            _mapper.Map(rq, userCr);

            _userRepository.Update(userCr);

            var dto = _mapper.Map<DataResponseUser>(userCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponseUser>.ResponseSuccess("Cập nhật thông tin thành công", dto);

        }

        public async Task<ResponseObject<DataResponseUser>> DeleteUser(Guid id)
        {
            var userCr = await _userRepository.GetByIdWithDetailsAsync(id)
             ?? throw new NotFoundException("User không tồn tại.");

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
                throw new NotFoundException($"User với ID '{userId}' trong token không tồn tại trong hệ thống.");

            if (!user.IsActive)
                throw new BadRequestException("Tài khoản của bạn đã bị khóa hoặc chưa được kích hoạt.");

            return user;
        }

        public async Task<User> FindOrCreateExternalUserAsync(string provider, string providerKey, string email, string? name, CancellationToken ct = default)
        {
            var existingLink = await _externalLoginRepository.GetExistingLink(provider,providerKey,ct);

            if (existingLink != null)
                return existingLink.User;

            var user = await _userRepository.GetByEmailOrNumberPhoneAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    UserName = name
                };
                _userRepository.Add(user);
            }

            var link = new ExternalLogin
            {
                Provider = provider,
                ProviderKey = providerKey,
                UserId = user.Id
            };
           _externalLoginRepository.Add(link);

           await  _unitOfWork.SaveChangesAsync();

           return user;

        }
    }
}
