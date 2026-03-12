using BetaCinema.Application.DTOs.Auth.External;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface IExternalAuthService
    {
        Task<ExternalAuthResult> HandleCallbackAsync(string provider, ClaimsPrincipal externalPrincipal, CancellationToken ct = default);

        
    }
}
