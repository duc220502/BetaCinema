using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Seats;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.Seats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface ISeatService
    {
        Task<ResponseObject<DataResponseSeat>> AddSeatAsync(Request_AddSeat rq);

        Task<ResponseObject<DataResponseSeat>> GetSeatByIdAsync(Guid id);

        Task<ResponseObject<DataResponseSeat>> UpdateSeat(Guid id, Request_UpdateSeat rq);


        Task<ResponseObject<DataResponseSeat>> DeleteSeat(Guid id);

        void UpdateSeatsAfterPurchase(List<Seat> seats);
    }
}
