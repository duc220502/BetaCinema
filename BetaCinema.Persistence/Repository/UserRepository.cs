using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Persistence.DBContext;
using BetaCinema.Persistence.Extensions;
using BetaCinema.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class UserRepository(AppDbContext context) : BaseRepository<User>(context), IUserRepository
    {
        public async Task<bool> CheckDupplicateUser(string userName, string email, string numberPhone, CancellationToken ct = default)
            => await _context.Users.AnyAsync(x => x.Email == email || x.UserName == userName || x.NumberPhone == numberPhone);

        public async Task<User?> GetByInformationLoginAsync(string userLogin, CancellationToken ct = default)

           =>  await _context.Users.Include(x => x.Role) 
                    .FirstOrDefaultAsync(x =>x.UserStatusId == (int)Domain.Enums.UserStatus.Active  && x.UserName == userLogin || x.Email == userLogin || x.NumberPhone == userLogin, ct);

        public async Task<User?> GetByEmailOrNumberPhoneAsync(string account, CancellationToken ct = default)

          => await _context.Users.FirstOrDefaultAsync(x => x.IsActive && x.Email == account || x.NumberPhone == account, ct);
       
        public async Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)

          => await _context.Users.Include(u => u.Role).Include(u => u.UserStatus).Include(u=>u.RankCustomer)
            .FirstOrDefaultAsync(u => u.Id == id, ct);


        public async Task<PageResult<User>>  GetAllUsersAsync(Pagination pagination)

          =>  await _context.Users.Include(u => u.Role).Include(u => u.UserStatus).ToPagedListAsync(pagination);


        public async Task<bool> IsEmailUniqueAsync(string? email, Guid currentUserId)
            
          => !await _context.Users.AnyAsync(u => u.Email == email && u.Id != currentUserId);
    

        public async Task<bool> IsUserNameUniqueAsync(string? userName, Guid currentUserId)

          => !await _context.Users.AnyAsync(u => u.UserName == userName && u.Id != currentUserId);

        public async Task<bool> IsNumberPhoneUniqueAsync(string? numberPhone, Guid currentUserId)

          => !await _context.Users.AnyAsync(u => u.NumberPhone == numberPhone && u.Id != currentUserId);
    }
}
