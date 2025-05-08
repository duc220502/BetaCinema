using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IAuthService
    {
        Task<ResponseObject<DataResponseUser>> Register(Request_Register rq);
        Task<ResponseObject<DataResponseToken>> Login(Request_Login rq);

        Task<ResponseObject<DataResponseToken>> RenewToken(Request_RenewAccessToken rq);

    }
}
