using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class ExternalLoginRepository(AppDbContext context) : BaseRepository<ExternalLogin>(context), IExternalLoginRepository
    {
        public async Task<ExternalLogin?> GetExistingLink(string provider, string providerKey, CancellationToken ct = default)
        => await _context.ExternalLogins.Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Provider == provider && x.ProviderKey == providerKey, ct);
    }
}
