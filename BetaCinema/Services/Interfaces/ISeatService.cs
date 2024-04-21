using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface ISeatService
    {
        public ResponseObject<IEnumerable<DataResponseSeat>> CreateSeat(int roomId, IEnumerable<Request_AddSeat> rqs);
        ResponseObject<DataResponseSeat> UpdateSeat(int id, Request_UpdateSeat rq);

        ResponseObject<DataResponseSeat> DeleteSeat(int id);
    }
}
