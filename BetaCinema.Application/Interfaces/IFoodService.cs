using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Application.DTOs.DataRequest.Cinemas;
using BetaCinema.Application.DTOs.DataRequest.Foods;
using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IFoodService
    {
        Task<ResponseObject<DataResponseFood>> AddFood(Request_AddFood rq);
        Task<ResponseObject<DataResponseFood>> GetFoodById(Guid id);

        Task<ResponseObject<DataResponseFood>> UpdateFood(Guid id,Request_UpdateFood rq);

        Task<ResponseObject<DataResponseFood>> DeleteFood(Guid id);

        Task<List<PreparedFoodDto>> PrepareFoodItemsAsync(List<Request_FoodItem>? requestItems);
    }
}
