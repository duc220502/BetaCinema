using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Foods
{
    public class AddFoodValidator : AbstractValidator<Request_AddFood>
    {
        private readonly IFoodRepository _foodRepository;
        public AddFoodValidator(IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository;

            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Name)
                .NotEmpty().NotNull().WithMessage("Tên Food không được để trống")
                .MustAsync(BeUniqueFoodName).WithMessage("Foodname này đã được sử dụng.");

            RuleFor(x => x.Price)
               .GreaterThanOrEqualTo(0).WithMessage("Giá tiền không được nhỏ hơn 0.")
               .When(x => x.Price.HasValue);
        }

        private async Task<bool> BeUniqueFoodName(string? name, CancellationToken cancellationToken)
        {
            return await _foodRepository.IsFoodNameUniqueAsync(name);
        }
    }
}
