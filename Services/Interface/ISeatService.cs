using BetaCinema.Entities;
using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface ISeatService
    {
        Task<ResponseObject<DataResponseSeat>> AddSeat(Request_AddSeat rq);
        Task<ResponseObject<DataResponseSeat>> UpdateSeat(Request_UpdateSeat rq);

        Task<ResponseObject<DataResponseSeat>> DeleteSeat(int id);

        Task<ResponseObject<List<DataResponseSeat>>> ListSeatOfRooms(Pagination pagination,int roomId);   
    }
}
