using BetaCinema.Application.DTOs.Auth.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Catching
{
    public interface IExternalLinkingStore
    {
        Task SaveStateAsync(string linkingToken, ExternalLinkState state, TimeSpan ttl, CancellationToken ct);
        Task<ExternalLinkState?> GetStateAsync(string linkingToken, CancellationToken ct);
        Task DeleteStateAsync(string linkingToken, CancellationToken ct);

        Task SaveOtpAsync(string linkingToken, string otpPlain, TimeSpan ttl, CancellationToken ct);
        Task<bool> VerifyOtpAsync(string linkingToken, string otpPlain, CancellationToken ct);
        Task<long> IncrementFailCountAsync(string linkingToken, CancellationToken ct);
        Task MarkUsedAsync(string linkingToken, CancellationToken ct);
        Task<bool> IsUsedAsync(string linkingToken, CancellationToken ct);
    }
}
