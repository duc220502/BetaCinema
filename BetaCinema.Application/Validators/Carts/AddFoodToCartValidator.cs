using BetaCinema.Application.DTOs.Cart;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Carts
{
    public class AddFoodToCartValidator : AbstractValidator<AddFoodToCartRequest>
    {
        private readonly IFoodRepository _foodRepository ;

        public AddFoodToCartValidator( IFoodRepository foodRepository)
        {
            _foodRepository = foodRepository ;

            RuleFor(x => x.Quantity)
              .NotNull().NotEmpty().WithMessage("Số lượng food không được để trống.")
              .GreaterThanOrEqualTo(1).WithMessage("Số lượng food không hợp lí.");

            RuleFor(x => x.FoodId)
            .NotEqual(Guid.Empty).WithMessage("ID là bắt buộc và không được để trống.")
            .MustAsync(CheckFood).WithMessage("MovieType không tồn tại");
        }

        private async Task<bool> CheckFood(Guid id, CancellationToken cancellationToken)
          => await _foodRepository.GetByIdAsync(id) != null;
    }
}
