using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Cinemas
{
    public class UpdateCinemaValidator : AbstractValidator<Request_UpdateCinema>
    {
        public UpdateCinemaValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .MinimumLength(8).WithMessage("UserName phải có ít nhất 8 ký tự")
            .MaximumLength(200).WithMessage("Tên không được vượt quá 200 ký tự.")
            .When(x => x.Name != null); 

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ không được để trống.")
                .When(x => x.Address != null);

        }
    }
}
