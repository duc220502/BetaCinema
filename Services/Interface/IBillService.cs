using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IBillService
    {
        Task<ResponseObject<DataResponseBill>> AddBill(int UserId,Request_AddBill rq);
    }
}
