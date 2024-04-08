using BetaCinema.Entities;
using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IAuthService
    {
        DataResponseToken GenerateAccessToken(User user);
        public ResponseObject<DataResponseToken> RenewAccessToken(Request_RenewAccessToken rq);
        ResponseObject<DataResponseUser> Register(Request_Register rq);
        ResponseObject<DataResponseToken> Login(Request_Login rq);
        ResponseObject<DataResponseConfirmEmail> ConfirmEmail(string code);

        IEnumerable<DataResponseUser> get_User(Pagination? pagination);
    }
}
