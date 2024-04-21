using BetaCinema.Payloads.DataRequest;
using BetaCinema.Payloads.DataResponses;
using BetaCinema.Payloads.Responses;

namespace BetaCinema.Services.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext content,Request_VnPayment rq);
        ResponseObject<VnPaymentResponseModel> PaymentExecute(IQueryCollection collections);

    }
}
