using BetaCinema.Application.DTOs.DataRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class VerifyCodeValidator: AbstractValidator<Request_VerifyCode>
    {
        public VerifyCodeValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("UserId  không được để trống");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Mã xác thực không được để trống");

        }
    }
}
