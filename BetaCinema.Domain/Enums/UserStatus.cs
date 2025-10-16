using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Enums
{
    public enum UserStatus
    {
        PendingActivation = 1,
        Active = 2,
        Locked = 3,
        Banned = 4,
        Deactivated = 5
    }
}
