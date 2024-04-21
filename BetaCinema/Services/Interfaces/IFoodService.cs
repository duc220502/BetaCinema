using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IFoodService
    {
        ResponseObject<DataResponseFood> CreateFood(Request_AddFood rq);
        ResponseObject<DataResponseFood> UpdateFood(int id, Request_UpdateFood rq);

        ResponseObject<DataResponseFood> DeleteFood(int id);

    }
}
