using BetaCinema.Application.Interfaces.Catching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public sealed class IdempotencyService(IConnectionMultiplexer mux) : IIdempotencyService
    {
        readonly IDatabase _db = mux.GetDatabase();
        public async Task<bool> EnsureOnceAsync(string key, TimeSpan ttl)
        => await _db.StringSetAsync($"idemp:{key}", "1", ttl, When.NotExists);
    }
}
