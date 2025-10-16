using BetaCinema.Application.DTOs.DataRequest;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Users
{
    public class RenewTokenValidator: AbstractValidator<Request_RenewToken>
    {
        public RenewTokenValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.AccessToken)
                .NotEmpty().WithMessage("AccessToken không được để trống");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("RefreshToken không được để trống");
                
        }
    }
}
