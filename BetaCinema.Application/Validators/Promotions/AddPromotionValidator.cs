using BetaCinema.Application.DTOs.DataRequest.Promotions;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Promotions
{
    public class AddPromotionValidator : AbstractValidator<Request_AddPromotion>
    {
        private readonly IRankCustomerRepository _rankCustomerRepository;
        public AddPromotionValidator(IRankCustomerRepository rankCustomerRepository) 
        {
            _rankCustomerRepository = rankCustomerRepository;
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Scope)
              .IsInEnum().WithMessage("Giá trị của Scope không hợp lệ.");

            RuleFor(x => x.Scope)
            .Equal(PromotionScope.ByRank).WithMessage("Scope phải là 'ByRank' khi RankCustomerId được cung cấp.")
            .When(x => x.RankCustomerId.HasValue);

            RuleFor(x => x.DiscountType)
              .IsInEnum().WithMessage("Giá trị của DiscountType không hợp lệ.");

            RuleFor(x => x.DiscountValue)
              .GreaterThanOrEqualTo(0).WithMessage("DiscountValue không hợp lí.");

            RuleFor(x => x.MinBillValue)
              .GreaterThanOrEqualTo(0).WithMessage("MinBillValue không hợp lí.")
              .When(x => x.MinBillValue.HasValue);

            RuleFor(x => x.MaxDiscountValue)
              .GreaterThanOrEqualTo(0).WithMessage("MaxDiscountValue không hợp lí.")
              .When(x => x.MaxDiscountValue.HasValue);

            RuleFor(x => x.UsageLimit)
             .GreaterThanOrEqualTo(0).WithMessage("UsageLimit không hợp lí.");


            RuleFor(x => x.StartTime)
            .NotEmpty().WithMessage("Thời gian phát hành không được để trống")
            .GreaterThan(DateTime.UtcNow).WithMessage("Thời gian bắt đầu phải lớn hơn ngày hiện tại.");

            RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime).WithMessage("Thời gian kết thúc phải lớn hơn StartTime.")
            .When(x => x.EndTime.HasValue);

            RuleFor(x => x.RankCustomerId)
            .Must(id => Enum.IsDefined(typeof(UserRank), id!))
            .WithMessage("Giá trị RateId không hợp lệ hoặc không tồn tại.")
            .When(x=>x.RankCustomerId.HasValue );

            RuleFor(x => x.RankCustomerId)
           .NotEmpty().WithMessage("Phải cung cấp RankCustomerId khi Scope là 'ByRank'.")
           .When(x => x.Scope == PromotionScope.ByRank);
        }
    }
}
