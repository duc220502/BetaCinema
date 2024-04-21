using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IRoomService
    {
        ResponseObject<DataResponseRoom> CreateRoom(Request_AddRoom rq);
        ResponseObject<DataResponseRoom> UpdateRoom(int id,Request_UpdateRoom rq);

        ResponseObject<DataResponseRoom> DeleteRoom(int id);
    }
}
