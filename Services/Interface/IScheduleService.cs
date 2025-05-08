using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IScheduleService
    {
        Task<ResponseObject<DataResponseSchedule>> AddSchedule(Request_AddSchedule rq);
        Task<ResponseObject<DataResponseSchedule>> UpdateSchedule(Request_UpdateSchedule rq);

        Task<ResponseObject<DataResponseSchedule>> DeleteSchedule(int scheduleId);


    }
}
