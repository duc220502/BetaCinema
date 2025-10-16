using BetaCinema.Application.Enums;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IConfirmationCodeService
    {
        (ConfirmEmail confirmEmail, IConfirmationMethodStrategy strategy) CreateCode(Guid userId, ConfirmationMethod method, CodePurpose purpose);

    }
}
