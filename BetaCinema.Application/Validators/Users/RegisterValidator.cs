using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Users;
using BetaCinema.Application.DTOS.DataRequest.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class RegisterValidator : AbstractValidator<Request_Register>
    {
        private readonly IUserRepository _userRepository;
        public RegisterValidator(IUserRepository userRepository)
        {

            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName không được để trống")
                .MinimumLength(8).WithMessage("UserName phải có ít nhất 8 ký tự");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống")
                .Must(email => email.IsValidEmail())
                .WithMessage("Định dạng email không hợp lệ.");


            RuleFor(x => x.NumberPhone)
                .NotEmpty().WithMessage("Number không được để trống")
                .Must(numberphone=>  numberphone.IsValidPhoneNumber())
                .WithMessage("Định dạng số điện thoại không hợp lệ.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password không được để trống")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự");

            RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                return !await _userRepository.CheckDupplicateUser(request.UserName, request.Email, request.NumberPhone);
            })
            .WithMessage("Tên đăng nhập, Email hoặc Số điện thoại đã được sử dụng.");

        }
    }
}
