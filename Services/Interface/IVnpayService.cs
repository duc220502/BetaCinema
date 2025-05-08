using BetaCinema.PayLoads.DataRequests;
using BetaCinema.PayLoads.DataResponses;

namespace BetaCinema.Services.Interface
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(Request_VnpayPayment model, HttpContext context);
        DataResponseVnpayPayment PaymentExecute(IQueryCollection collections);

    }
}
