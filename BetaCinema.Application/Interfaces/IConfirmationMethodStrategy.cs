using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IConfirmationMethodStrategy
    {
        string StrategyName { get; }

        string GenerateToken();

        TimeSpan GetExpirationTime();
    }
}
