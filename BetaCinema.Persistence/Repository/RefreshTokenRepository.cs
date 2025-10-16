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
    public class RefreshTokenRepository(AppDbContext context) : BaseRepository<RefreshToken>(context), IRefreshTokenRepository
    {
        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        => await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

        public async Task<RefreshToken?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
