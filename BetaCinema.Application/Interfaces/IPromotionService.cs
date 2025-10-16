using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Promotions;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IPromotionService
    {
        Task<ResponseObject<DataResponsePromotion>> AddPromotion(Request_AddPromotion rq);

        Task<ResponseObject<DataResponsePromotion>> GetPromotionById(Guid id);

        Task<ResponseObject<DataResponsePromotion>> UpdatePromotion(Guid id, Request_UpdatePromotion rq);


        Task<ResponseObject<DataResponsePromotion>> DeletePromotion(Guid id);

        Task<PromotionResult> ApplyPromotionsAsync(BillContext context, List<Guid>? promotionIds);

        void IncrementUsageCounts(List<Promotion> promotionsToUpdate);
    }
}
