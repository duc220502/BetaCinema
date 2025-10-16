using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IRoomService
    {
        Task<ResponseObject<DataResponseRoom>> AddRoom(Request_AddRoom rq);

        Task<ResponseObject<DataResponseRoom>> GetRoomById(Guid id);

        Task<ResponseObject<DataResponseRoom>> UpdateRoom(Guid id, Request_UpdateRoom rq);
        Task<ResponseObject<DataResponseRoom>> DeleteRoom(Guid id);
    }
}
