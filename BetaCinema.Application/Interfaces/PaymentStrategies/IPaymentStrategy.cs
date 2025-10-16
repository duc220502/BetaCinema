using BetaCinema.Application.DTOs;
using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.PaymentStrategies
{
    public interface IPaymentStrategy
    {
        string StrategyCode { get; }
        Task<PaymentInitiationResult> InitiatePaymentAsync(Bill bill);

        Task<PaymentConfirmationResult> ConfirmPaymentAsync(Dictionary<string, string> callbackData);

        Task<bool> VerifyTransactionOnGatewayAsync(Guid billId, decimal amount, string gatewayTransactionId, string payDate , string txnRef) => Task.FromResult(true); 

    }
}
