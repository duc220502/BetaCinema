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
    public class UserStatusRepository(AppDbContext context) : BaseRepository<UserStatus>(context), IUserStatusRepository
    {
       /* public async Task<UserStatus?> GetDefaultStatusAsync()
        => await _context.UserStatuses.FirstOrDefaultAsync(r => r.Code == "Status2");

        public async Task<Guid?> GetIdStatusLoginAsync()
       => (await _context.UserStatuses.FirstOrDefaultAsync(r => r.Code == "Status1"))?.Id;*/
    }
}
