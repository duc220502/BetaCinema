using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class ConfirmEmailRepository(AppDbContext context) : BaseRepository<ConfirmEmail>(context), IConfirmEmailRepository
    {
        public async Task<ConfirmEmail?> GetByAccountAndCodeAsync(string account, string code, CancellationToken ct = default)
        {
            var userId = await _context.Users
                         .Where(u => u.Email == account || u.NumberPhone == account)
                         .Select(u => u.Id) 
                         .FirstOrDefaultAsync(ct);

            if (userId == Guid.Empty)
            {
                return null;
            }

            return await _context.ConfirmEmails
                .FirstOrDefaultAsync(x => x.IsConfirm == false && x.ConfirmCode == code && x.UserId == userId,ct);
        }

        public async Task<ConfirmEmail?> GetByUserIdAndCodeAsync(Guid userId, string code, CancellationToken ct = default)
        => await _context.ConfirmEmails
                .FirstOrDefaultAsync(x => x.IsConfirm == false && x.ConfirmCode == code && x.UserId == userId, ct);
    }
}
