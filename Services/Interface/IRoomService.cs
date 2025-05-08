using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IRoomService
    {
        Task<ResponseObject<DataResponseRoom>> AddRoom(Request_AddRoom rq);
        Task<ResponseObject<DataResponseRoom>> UpdateRoom(Request_UpdateRoom rq);

        Task<ResponseObject<DataResponseRoom>> DeleteRoom(int roomId);
        
    }
}
