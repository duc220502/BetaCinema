using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface ICurrentUserservice
    {
        Guid? UserId { get; }
        string? Role { get; }

        Guid GetRequiredUserId();

        string GetRequiredRoleName();

    }
}
