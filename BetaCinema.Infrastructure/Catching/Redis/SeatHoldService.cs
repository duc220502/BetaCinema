using BetaCinema.Application.Interfaces.Catching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Catching.Redis
{
    public sealed class SeatHoldService(IConnectionMultiplexer mux) : ISeatHoldService
    {
        private readonly IDatabase db = mux.GetDatabase();
        public Task<bool> ReleaseAsync(int showId, string seat)
        =>db.KeyDeleteAsync($"hold:show:{showId}:seat:{seat}");

        public Task<bool> TryHoldAsync(int showId, string seat, TimeSpan ttl)
        => db.StringSetAsync($"hold:show:{showId}:seat:{seat}", "1", ttl, When.NotExists);
    }
}
