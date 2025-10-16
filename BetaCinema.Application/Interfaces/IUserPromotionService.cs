using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.UserPromotions;
using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IUserPromotionService
    {
        Task<ResponseObject<DataResponseUserPromotionPersonal>> AddUserPromotionAsync(Request_AddUserPromotion rq);

        Task<ResponseObject<DataResponseUserPromotionPersonal>> GetUserPromotionByIdAsync(Guid id);

        Task<ResponseObject<DataResponseUserPromotionPersonal>> UpdateUserPromotion(Guid id, Request_UpdateUserPromotion rq);

        Task<ResponseObject<DataResponseUserPromotionPersonal>> DeleteUserPromotion(Guid id);
    }
}
