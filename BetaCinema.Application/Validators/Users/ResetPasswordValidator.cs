using BetaCinema.Application.DTOs.DataRequest.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class ResetPasswordValidator: AbstractValidator<Request_ResetPassword>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới không được để trống")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự");

        }
    }
}
