using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.UserPromotions
{
    public class AddUserPromotionValidator : AbstractValidator<Request_AddUserPromotion>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPromotionRepository _promotionRepository;
        public AddUserPromotionValidator(IPromotionRepository promotionRepository , IUserRepository userRepository) 
        {
            _promotionRepository = promotionRepository;
            _userRepository = userRepository;
            RuleFor(x => x).NotNull().WithMessage("Request body không được để trống.");

            RuleFor(x => x.Quantity)
              .GreaterThanOrEqualTo(0).WithMessage("Số lượng không hợp lí.")
              .When(x => x.Quantity.HasValue);

            RuleFor(x => x.UserId)
           .NotEmpty().WithMessage("UserId không được để trống khi được cung cấp.")
           .MustAsync(CheckUser).WithMessage("User  không tồn tại");

            RuleFor(x => x.PromotionId)
           .NotEmpty().WithMessage("PromotionId không được để trống khi được cung cấp.")
            .CustomAsync(async (promotionId, context, cancellationToken) =>
            {
                var promotion = await _promotionRepository.GetByIdAsync(promotionId);

                if (promotion == null)
                {
                    context.AddFailure("PromotionId", "Khuyến mãi không tồn tại.");
                    return; 
                }
                if (promotion.Scope != PromotionScope.Personal)
                {
                    context.AddFailure("PromotionId", "Chỉ có thể gán các khuyến mãi loại 'Personal'.");
                }
            });
        }

        private async Task<bool> CheckUser(Guid id, CancellationToken cancellationToken)
        => await _userRepository.GetByIdAsync(id) != null;
    }
}
