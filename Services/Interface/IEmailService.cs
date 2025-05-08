using BetaCinema.Handle;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;

namespace BetaCinema.Services.Interface
{
    public interface IEmailService
    {
        Task<ResponseObject<DataResponseEmailMessage>>  SendMail(EmailMessage message);
        Task<ResponseObject<string>> ConfirmEmail(string mailCode);
    }
}
