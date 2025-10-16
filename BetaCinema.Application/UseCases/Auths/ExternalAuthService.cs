using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace BetaCinema.Application.UseCases.Auths
{
    public class ExternalAuthService(IConfiguration configuration , IUserService userService , IExternalIdentityNormalizer externalIdentityNormalizer) : IExternalAuthService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IUserService _userService = userService;
        private readonly IExternalIdentityNormalizer _normalizer = externalIdentityNormalizer;
        public async Task<User> HandleCallbackAsync(string provider, ClaimsPrincipal externalPrincipal, CancellationToken ct = default)
        {
            if (!(externalPrincipal?.Identity?.IsAuthenticated ?? false))
                throw new UnauthorizedAccessException("External principal not authenticated");

            var ext = _normalizer.Normalize(externalPrincipal, provider);

            if (provider.Equals("Google", StringComparison.OrdinalIgnoreCase))
            {
                var allowedDomain = _configuration["Authentication:Providers:Google:HostedDomain"];
                if (!string.IsNullOrEmpty(allowedDomain) &&
                   !ext.Email.EndsWith($"@{allowedDomain}", StringComparison.OrdinalIgnoreCase))
                    throw new UnauthorizedAccessException("Email domain not allowed");
            }

            // Map/tạo user nội bộ + liên kết ExternalLogins(provider, key)
            var user = await _userService.FindOrCreateExternalUserAsync(ext.Provider, ext.ProviderKey, ext.Email, ext.Name);
            return user;
        }
    }
}
