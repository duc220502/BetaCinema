using BetaCinema.Application.DTOs;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces.PaymentStrategies;
using BetaCinema.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public class CashPaymentStrategy : IPaymentStrategy
    {
        public string StrategyCode => Domain.Enums.PaymentMethod.CASH.ToString();

        public Task<PaymentConfirmationResult> ConfirmPaymentAsync(Dictionary<string, string> callbackData)
        {
            var billId = Guid.Parse(callbackData.GetValueOrDefault("billId") ?? throw new NotFoundException("Không tìm thấy billid"));
            var amount = decimal.Parse(callbackData.GetValueOrDefault("amount") ?? throw new NotFoundException("Không tìm thấy amount"));

            var result = new PaymentConfirmationResult
            {
                IsSuccess = true,
                BillId = billId,
                Amount = amount,
                PaymentGatewayTransactionId = null 
            };

            return Task.FromResult(result);
        }

        public Task<PaymentInitiationResult> InitiatePaymentAsync(Bill bill)
        {
            var result = new PaymentInitiationResult()
            {
                IsSuccess = true,
                Message = "Hóa đơn đã được tạo và đang chờ thanh toán tại quầy.",
                RedirectUrl = null,
                QrCodeData = null
            };

            return Task.FromResult(result);
        }
    }
}
