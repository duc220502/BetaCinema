using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface ITicketService
    {
        Task<ResponseObject<DataResponseTicket>> AddTicket(Request_AddTicket rq);

        Task<ResponseObject<DataResponseTicket>> UpdateTicket(Request_UpdateTicket rq);

        Task<ResponseObject<DataResponseTicket>> DeleteTicket(int ticketId);

        Task<ResponseObject<DataResponseTicket>> LockedTicket(int userId , Request_LockedTicket rq);
    }
}
