using BetaCinema.Application.Enums;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class ConfirmationCodeService(IEnumerable<IConfirmationMethodStrategy> strategies) : IConfirmationCodeService
    {
        private readonly IEnumerable<IConfirmationMethodStrategy> _strategies = strategies;

        public (ConfirmEmail confirmEmail, IConfirmationMethodStrategy strategy) CreateCode(Guid userId, ConfirmationMethod method, CodePurpose purpose)
        {
            var strategy = _strategies.FirstOrDefault(s => s.StrategyName.Equals(method.ToString(), StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("Phương thức xác thực không được hỗ trợ.");

            var token = strategy.GenerateToken();

            var confirmEmail = new ConfirmEmail
            {
                ConfirmCode = token,
                ExpiredTime = DateTime.UtcNow.Add(strategy.GetExpirationTime()),
                IsConfirm = false,
                Purpose = purpose,
                UserId = userId
            };

            return (confirmEmail, strategy);
        }
    }
}
