using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Promotions;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace BetaCinema.Application.UseCases
{
    public class PromotionService(IPromotionRepository promotionRepository,IMapper mapper ,IUnitOfWork unitOfWork) : IPromotionService
    {

        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPromotionRepository _promotionRepository = promotionRepository;
        public async Task<ResponseObject<DataResponsePromotion>> AddPromotion(Request_AddPromotion rq)
        {
            var newPromotion = new Promotion()
            {
                Name = rq.Name,
                Description= rq.Description,
                Scope = rq.Scope,
                DiscountType = rq.DiscountType,
                DiscountValue = rq.DiscountValue,
                MinBillValue = rq.MinBillValue??0,
                MaxDiscountValue = rq.MaxDiscountValue??0,
                UsageLimit = rq.UsageLimit,
                CurrentUsage = 0,
                StartTime = rq.StartTime,
                EndTime = rq.EndTime,
                IsActive = true,
                RankCustomerId = rq.RankCustomerId,

            };

            _promotionRepository.Add(newPromotion);


            var dto = _mapper.Map<DataResponsePromotion>(newPromotion);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponsePromotion>.ResponseSuccess("Thêm Promotion thành công", dto);
        }

        public async Task<PromotionResult> ApplyPromotionsAsync(BillContext context, List<Guid>? promotionIds)
        {
            var validPromotions = await ValidatePromotionsInternalAsync(context, promotionIds);

            var result = CalculateDiscountInternal(context.SubTotal, validPromotions);

            return result;
        }

        public async Task<ResponseObject<DataResponsePromotion>> DeletePromotion(Guid id)
        {
            var promotionCr = await _promotionRepository.GetByIdAsync(id)
             ?? throw new NotFoundException("Promotion không tồn tại.");

            promotionCr.IsActive = false;

            _promotionRepository.Update(promotionCr);

            var dto = _mapper.Map<DataResponsePromotion>(promotionCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponsePromotion>.ResponseSuccess("Xóa Promotion thành công", dto);
        }

        public async Task<ResponseObject<DataResponsePromotion>> GetPromotionById(Guid id)
        {
            var result = await _promotionRepository.GetByIdAsync(id) ?? throw new NotFoundException("Không tìm thấy Promotion");

            var dto = _mapper.Map<DataResponsePromotion>(result);

            return ResponseObject<DataResponsePromotion>.ResponseSuccess("Lấy thông tin promotion thành công ", dto);
        }

      

        public async Task<ResponseObject<DataResponsePromotion>> UpdatePromotion(Guid id, Request_UpdatePromotion rq)
        {
            var promotionCr = await _promotionRepository.GetByIdAsync(id)
            ?? throw new NotFoundException($"Không tìm thấy Promotion với ID: {id}");

            _mapper.Map(rq, promotionCr);

            _promotionRepository.Update(promotionCr);

            var dto = _mapper.Map<DataResponsePromotion>(promotionCr);

            await _unitOfWork.SaveChangesAsync();

            return ResponseObject<DataResponsePromotion>.ResponseSuccess("Cập nhật thông tin thành công", dto);
        }



        private async Task<List<Promotion>> ValidatePromotionsInternalAsync(BillContext context, List<Guid>? promotionIds)
        {
            if (promotionIds == null || !promotionIds.Any())
                return new List<Promotion>();

            var validPromotions = new List<Promotion>();

            var distinctIds = promotionIds.Distinct().ToList();
            var promosFromDb = await _promotionRepository.GetPromotionsByListIdAsync(distinctIds);

            foreach (var id in distinctIds)
            {
                var promo = promosFromDb?.FirstOrDefault(p => p.Id == id);

                if (promo == null)
                    throw new InvalidOperationException($"Khuyến mãi '{id}' không tồn tại.");
                if (!promo.IsActive)
                    throw new InvalidOperationException($"Khuyến mãi '{id}' đã bị vô hiệu hóa.");
                if (DateTime.UtcNow < promo.StartTime || DateTime.UtcNow > promo.EndTime)
                    throw new InvalidOperationException($"Khuyến mãi '{id}' đã hết hạn hoặc chưa tới ngày áp dụng.");
                if (promo.CurrentUsage >= promo.UsageLimit)
                    throw new InvalidOperationException($"Khuyến mãi '{id}' đã hết lượt sử dụng.");
                if (context.SubTotal < promo.MinBillValue)
                    throw new InvalidOperationException($"Hóa đơn chưa đạt giá trị tối thiểu để dùng khuyến mãi '{id}'.");

                validPromotions.Add(promo);
            }

            

            return validPromotions;
        }

        private PromotionResult CalculateDiscountInternal(decimal subTotal, List<Promotion> validPromotions)
        {
            if (!validPromotions.Any())
            {
                return new PromotionResult { SubTotal = subTotal, FinalTotal = subTotal, TotalDiscountAmount = 0 };
            }

            decimal currentTotal = subTotal;
            decimal totalDiscount = 0;
            var appliedPromotionDetails = new List<AppliedPromotionDetailDto>();

            foreach (var promo in validPromotions)
            {
                decimal discountAmountThisStep = 0;
                if (promo.DiscountType == DiscountType.FixedAmount) // Giảm tiền cố định
                {
                    discountAmountThisStep = promo.DiscountValue;
                }
                else if (promo.DiscountType == DiscountType.Percentage) // Giảm %
                {
                    discountAmountThisStep = currentTotal * (promo.DiscountValue / 100);
                }

                discountAmountThisStep = Math.Round(discountAmountThisStep);

                currentTotal -= discountAmountThisStep;
                totalDiscount += discountAmountThisStep;

                appliedPromotionDetails.Add(new AppliedPromotionDetailDto() { PromotionId = promo.Id, Name = promo.Name, DiscountAmount = discountAmountThisStep });
            }

            return new PromotionResult
            {
                SubTotal = subTotal,
                FinalTotal = currentTotal > 0 ? currentTotal : 0,
                TotalDiscountAmount = totalDiscount,
                AppliedPromotions = appliedPromotionDetails
            };
        }

        public void IncrementUsageCounts(List<Promotion> promotionsToUpdate)
        {

            foreach (var promo in promotionsToUpdate)
            {
                promo.CurrentUsage++;
            }
        }
    }
}
