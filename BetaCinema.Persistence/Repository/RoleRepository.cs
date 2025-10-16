using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class RoleRepository(AppDbContext context) : BaseRepository<Role>(context), IRoleRepository
    {
     /*   public async Task<Role?> GetDefaultUserRoleAsync()
         => await _context.Roles.FirstOrDefaultAsync(r => r.Code == "User");*/
    }
}
