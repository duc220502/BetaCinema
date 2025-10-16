using BetaCinema.Application.DTOs.DataRequest.Foods;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Foods
{
    public class UpdateFoodValidator : AbstractValidator<Request_UpdateFood>
    {
        public UpdateFoodValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống.")
            .When(x => x.Name != null);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá tiền không được nhỏ hơn 0.")
                .When(x => x.Price.HasValue);

        }
    }
}
