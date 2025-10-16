using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces.Auths
{
    public interface ITokenValidator
    {
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}
