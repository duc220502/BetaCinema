using BetaCinema.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface IExternalIdentityNormalizer
    {
        ExternalIdentity Normalize(ClaimsPrincipal principal, string provider);
    }
}
