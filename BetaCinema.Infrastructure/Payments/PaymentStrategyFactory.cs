using BetaCinema.Application.Interfaces.PaymentStrategies;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Payments
{
    public class PaymentStrategyFactory(IEnumerable<IPaymentStrategy> strategies) : IPaymentStrategyFactory
    {
        private readonly IEnumerable<IPaymentStrategy> _strategies = strategies;

        public IPaymentStrategy GetStrategy(PaymentMethod method)
        {

            string strategyCode = method.ToString();
            var strategy = _strategies.FirstOrDefault(s => s.StrategyCode.Equals(strategyCode, StringComparison.OrdinalIgnoreCase));
            if (strategy == null)
                throw new NotSupportedException($"Hình thức thanh toán '{strategyCode}' không được hỗ trợ.");
            return strategy;
        }
    }
}
