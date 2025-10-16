using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IConfirmEmailRepository : IRepository<ConfirmEmail>
    {
        Task<ConfirmEmail?> GetByAccountAndCodeAsync(string account,string code, CancellationToken ct = default);
        Task<ConfirmEmail?> GetByUserIdAndCodeAsync(Guid userId, string code, CancellationToken ct = default);
    }
}
