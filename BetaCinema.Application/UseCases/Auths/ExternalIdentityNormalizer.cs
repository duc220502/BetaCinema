using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases.Auths
{
    public class ExternalIdentityNormalizer : IExternalIdentityNormalizer
    {
        public ExternalIdentity Normalize(ClaimsPrincipal p, string provider)
        {
            var sub = p.FindFirst("sub")?.Value ?? p.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";
            var email = p.FindFirst(ClaimTypes.Email)?.Value ?? "";
            var name = p.FindFirst(ClaimTypes.Name)?.Value ?? email;
            return new ExternalIdentity() { Provider = provider  , ProviderKey = sub , Email = email , Name = name };
        }
    }
}
