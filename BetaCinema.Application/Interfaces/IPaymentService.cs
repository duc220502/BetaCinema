using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseObject<PaymentInitiationResult>> InitiatePaymentAsync(Guid billId, Domain.Enums.PaymentMethod paymentMethod);
        Task<ResponseObject<object>> ProcessVnpayIpnAsync(Dictionary<string, string> vnpayResponseData);

        Task HandleSuccessfulPaymentAsync(Bill bill, string? paymentGatewayTransactionId);

        Task<ResponseObject<object>> ProcessCashPaymentConfirmationAsync(Guid billId);
    }
}
