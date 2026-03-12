using BetaCinema.Application.DTOs.Auth.Requests;
using BetaCinema.Application.DTOs.DataRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators
{
    public class ConfirmExternalRequestValidator : AbstractValidator<ConfirmExternalLinkRequest>
    {
        public ConfirmExternalRequestValidator()
        {
            RuleFor(x => x.LinkingToken)
           .NotEmpty().WithMessage("LinkingToken không được để trống.")
           .Length(32).WithMessage("LinkingToken không đúng định dạng.")
           .Matches("^[a-fA-F0-9]{32}$").WithMessage("LinkingToken không đúng định dạng.");

            RuleFor(x => x.Otp)
                .NotEmpty().WithMessage("OTP không được để trống.")
                .Length(6).WithMessage("OTP phải gồm đúng 6 chữ số.")
                .Matches(@"^\d{6}$").WithMessage("OTP phải là 6 chữ số.");
        }
    }
}
