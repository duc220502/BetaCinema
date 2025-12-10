using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Catching
{
    public interface IIdempotencyService
    {
        Task<bool> EnsureOnceAsync(string key, TimeSpan ttl);
    }

}
