using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.PaymentStrategies
{
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetStrategy(PaymentMethod method);
    }
}
