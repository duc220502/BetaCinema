using BetaCinema.Application.DTOs.DataRequest.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class LoginValidator : AbstractValidator<Request_Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserLogin)
                .NotEmpty().WithMessage("Thông tin đăng nhập không được để trống")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự");
        }
    }
}
