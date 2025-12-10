using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Catching
{
    public interface IRateLimiter
    {
        Task<bool> HitAsync(string route, string subject, int limit, TimeSpan window);
    }
}
