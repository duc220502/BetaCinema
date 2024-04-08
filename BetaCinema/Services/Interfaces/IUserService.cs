using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IUserService
    {
        ResponseObject<DataResponseUser> ChangPassword(int id, string oldPassword, string newPassword);
        ResponseObject<DataResponseUser> ForgotPassword(int id);
        ResponseObject<DataResponseUser> NewPassword(string code,string newPass);

    }
}
