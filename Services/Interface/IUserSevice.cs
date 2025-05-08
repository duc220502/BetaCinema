using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IUserSevice
    {
       Task<ResponseObject<DataResponseUser>>  ChangPassword(int id , Request_ChangPassword rq);
       Task<ResponseObject<DataResponseUser>>  NewPassword(int id, string newPassword);

    }
}
