using BetaCinema.Application.DTOs.DataRequest.Promotions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Promotions
{
    public class UpdatePromotionValidator : AbstractValidator<Request_UpdatePromotion>
    {

        public UpdatePromotionValidator() 
        {
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Tên không được để trống.")
               .When(x => x.Name != null);

            RuleFor(x => x.DiscountValue)
               .GreaterThanOrEqualTo(0).WithMessage("DiscountValue không hợp lí.")
               .When(x => x.DiscountValue.HasValue);

            RuleFor(x => x.MinBillValue)
              .GreaterThanOrEqualTo(0).WithMessage("MinBillValue không hợp lí.")
              .When(x => x.MinBillValue.HasValue);


            RuleFor(x => x.MaxDiscountValue)
              .GreaterThanOrEqualTo(0).WithMessage("MaxDiscountValue không hợp lí.")
              .When(x => x.MaxDiscountValue.HasValue);

            RuleFor(x => x.UsageLimit)
              .GreaterThanOrEqualTo(0).WithMessage("UsageLimit không hợp lí.")
              .When(x => x.UsageLimit.HasValue);

            RuleFor(x => x.StartTime)
               .NotEmpty().WithMessage("Thời gian phát hành không được để trống")
               .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải lớn hơn ngày hiện tại.")
               .When(x=>x.StartTime.HasValue);

            RuleFor(x => x.EndTime)
               .NotEmpty().WithMessage("Thời gian kết thúc không được để trống")
               .GreaterThan(x => x.StartTime).WithMessage("Thời gian kết thúc phải lớn hơn StartTime.")
               .When(x => x.EndTime.HasValue);
        }
    }
}
