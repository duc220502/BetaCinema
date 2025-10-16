using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Cinemas
{
    public class AddCinemaValidator : AbstractValidator<Request_AddCinema>
    {
        private readonly ICinemaRepository _cinemaRepository;
        public AddCinemaValidator(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên Cinema không được để trống")
                .MaximumLength(200).WithMessage("Tên không được vượt quá 200 ký tự.")
                .MustAsync(BeUniqueCinemaName).WithMessage("Tên cinema đã tồn tại");
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ không được để trống");
        }

        private async Task<bool> BeUniqueCinemaName(string? name, CancellationToken cancellationToken)
        {
            return await _cinemaRepository.IsNameUniqueAsync(name);
        }
    }
}
