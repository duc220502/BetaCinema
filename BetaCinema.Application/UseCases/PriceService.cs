using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class PriceService(IPriceRepository priceRepository) : IPriceService
    {
        private readonly IPriceRepository _priceRepository = priceRepository;
        public async Task<decimal> CalculateBaseTicketPriceAsync(Schedule schedule)
        {
            if (schedule.Movie == null)
                throw new InvalidOperationException("Thông tin phim không được tải cho suất chiếu này.");


            decimal currentPrice = schedule.Movie.BasePrice;

            var bestAdjustmentRule =   await _priceRepository.FindBestMatchingRuleAsync(schedule.StartAt) ?? 
                throw  new NotFoundException("Không tìm thấy khung giá phù hợp cho suất chiếu này.");

            if (bestAdjustmentRule != null)
            {

                if (bestAdjustmentRule.AdjustmentType == AdjustmentType.FixedAmount) 
                {
                    currentPrice += bestAdjustmentRule.AdjustmentValue; 
                }
                else if (bestAdjustmentRule.AdjustmentType == AdjustmentType.Percentage) 
                {
                    currentPrice += currentPrice * (bestAdjustmentRule.AdjustmentValue / 100);
                }
            }

            return currentPrice > 0 ? currentPrice : 0;
        }
    }
}
