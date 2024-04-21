using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IBillService
    {
        ResponseObject<DataResponseBill> CreateBill(int id,Request_AddBill rq);
    }
}
