using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Catching
{
    public interface ISeatHoldService
    {

        Task<bool> TryHoldAsync(int showId, string seat, TimeSpan ttl);
        Task<bool> ReleaseAsync(int showId, string seat);
    }
}
