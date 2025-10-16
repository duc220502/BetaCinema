using BetaCinema.Application.DTOs.DataRequest.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class ChangePasswordValidator : AbstractValidator<Request_ChangePassword>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPass)
                .NotEmpty().WithMessage("Mật khẩu cũ không được để trống")
                .MinimumLength(8).WithMessage("Phải ít nhất 8 ký tự");

            RuleFor(x => x.NewPass)
                .NotEmpty().WithMessage("Mật khẩu mới không được để trống")
                .MinimumLength(8).WithMessage("Phải ít nhất 8 ký tự");
        }
    }
}
