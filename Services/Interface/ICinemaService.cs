using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface ICinemaService
    {
        Task<ResponseObject<DataResponseCinema>> AddCinema(Request_AddCinema rq);
        Task<ResponseObject<DataResponseCinema>> UpdateCinema(Request_UpdateCinema rq);
        
        Task<ResponseObject<DataResponseCinema>> DeleteCinema(int id);

    }
}
