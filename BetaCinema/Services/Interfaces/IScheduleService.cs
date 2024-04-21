using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IScheduleService
    {
        ResponseObject<DataResponseSchedule> CreateSchedule(Request_AddSchedule rq);
        ResponseObject<DataResponseSchedule> UpdateSchedule(int id,Request_UpdateSchedule rq);
        ResponseObject<DataResponseSchedule> DeleteSchedule(int id);
    }
}
