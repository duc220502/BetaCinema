using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class UpdateMyProfileValidator : AbstractValidator<Request_UpdateMyProfile>
    {
        private readonly IUserRepository _userRepository ;
        private readonly ICurrentUserservice _currentUserService ;

        public UpdateMyProfileValidator(IUserRepository userRepository, ICurrentUserservice currentUserService)
        {
            _currentUserService = currentUserService;
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .MinimumLength(8).WithMessage("UserName phải có ít nhất 8 ký tự")
                .MustAsync(BeUniqueUserName).WithMessage("UserName này đã được sử dụng.")
                .When(x => !string.IsNullOrEmpty(x.UserName));
            RuleFor(x => x.Email)
                .Must(email => email!.IsValidEmail())
                .WithMessage("Định dạng email không hợp lệ.")
                .MustAsync(BeUniqueEmail).WithMessage("Email này đã được sử dụng.")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.NumberPhone)
                .Must(numberphone => numberphone!.IsValidPhoneNumber())
                .WithMessage("Định dạng số điện thoại không hợp lệ.")
                .MustAsync(BeUniqueNumberPhone).WithMessage("Sdt này đã được sử dụng.")
                .When(x => !string.IsNullOrEmpty(x.NumberPhone));

        }

        private async Task<bool> BeUniqueUserName(string? userName, CancellationToken cancellationToken)
        {
            return await _userRepository.IsUserNameUniqueAsync(userName, _currentUserService.GetRequiredUserId());
        }
        private async Task<bool> BeUniqueEmail(string? email, CancellationToken cancellationToken)
        {
            return await _userRepository.IsEmailUniqueAsync(email, _currentUserService.GetRequiredUserId());
        }
        private async Task<bool> BeUniqueNumberPhone(string? numberPhone, CancellationToken cancellationToken)
        {
            return await _userRepository.IsNumberPhoneUniqueAsync(numberPhone, _currentUserService.GetRequiredUserId());
        }
    }
}
