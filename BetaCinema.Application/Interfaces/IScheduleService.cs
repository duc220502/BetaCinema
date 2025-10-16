using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.DataRequest.Rooms;
using BetaCinema.Application.DTOs.DataRequest.Schedule;
using BetaCinema.Application.DTOs.DataResponse;
using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IScheduleService
    {
        public Task<ResponseObject<DataResponseSchedule>> AddSchedule(Request_AddSchedule rq);

        Task<ResponseObject<DataResponseSchedule>> GetScheduleById(Guid id);

        Task<ResponseObject<List<DataResponseAvailableSlotDto>>> GetAvailableSlotsAsync(Guid roomId, Guid movieId, DateTime date);

        Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Guid id, Request_UpdateSchedule rq);

        Task<ResponseObject<DataResponseSchedule>> DeleteSchedule(Guid id);

        Task<ResponseObject<List<DataResponseSeat>>> GetSeatsForScheduleAsync(Guid scheduleId);


    }

}
