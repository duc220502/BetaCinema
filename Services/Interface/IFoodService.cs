using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IFoodService
    {
        Task<ResponseObject<DataResponseFood>> AddFood(Request_AddFood rq);
        Task<ResponseObject<DataResponseFood>> UpdateFood(Request_UpdateFood rq);

        Task<ResponseObject<DataResponseFood>> DeleteFood(int id);

    }
}
