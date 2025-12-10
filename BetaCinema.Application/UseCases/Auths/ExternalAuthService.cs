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

            switch (provider.ToLowerInvariant())
            {
                case "google":
                    var allowedDomain = _configuration["Authentication:Providers:google:HostedDomain"];
                    if (!string.IsNullOrEmpty(allowedDomain) &&
                        !ext.Email.EndsWith($"@{allowedDomain}", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new UnauthorizedAccessException($"Only {allowedDomain} emails are allowed");
                    }

                    if (string.IsNullOrEmpty(ext.Email))
                    {
                        throw new UnauthorizedAccessException("Email is required from Google");
                    }
                    break;

                case "facebook":
                    // Facebook có thể không có email
                    //if (string.IsNullOrEmpty(ext.Email))
                    //{
                    //    // Option 1: Reject user không có email
                    //    //throw new UnauthorizedAccessException("Email permission is required");

                    //    // Option 2: Tạo email giả (không recommend)
                    //    // ext.Email = $"{ext.ProviderKey}@facebook.temp";

                    //    // Option 3: Cho phép null, xử lý sau
                    //    // (cần sửa User entity cho phép Email nullable)


                    //    //ext.Email = "";
                    //}
                    break;

                default:
                    // Provider khác (GitHub, Twitter, etc.)
                    if (string.IsNullOrEmpty(ext.Email))
                    {
                        throw new UnauthorizedAccessException($"Email is required from {provider}");
                    }
                    break;
            }

            var user = await _userService.FindOrCreateExternalUserAsync(ext.Provider, ext.ProviderKey, ext.Email, ext.Name);
            return user;
        }
    }
}
