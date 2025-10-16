using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.UserPromotions
{
    public class UpdateUserPromotionValidator : AbstractValidator<Request_UpdateUserPromotion>
    {
        public UpdateUserPromotionValidator() 
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Quantity)
             .GreaterThanOrEqualTo(0).WithMessage("Số lượng không hợp lí.")
             .When(x => x.Quantity.HasValue);

        }
    }
}
