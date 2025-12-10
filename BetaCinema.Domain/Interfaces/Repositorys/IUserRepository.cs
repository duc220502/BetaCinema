using BetaCinema.Domain.Entities.Users;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IUserRepository: IRepository<User>
    {
        Task<bool> CheckDupplicateUser(string userName, string email, string numberPhone, CancellationToken ct = default);

        Task<User?> GetByInformationLoginAsync(string userLogin , CancellationToken ct = default);
        Task<User?> GetByEmailOrNumberPhoneAsync(string account, CancellationToken ct = default);

        Task<User?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);

        Task<PageResult<User>> GetAllUsersAsync(Pagination pagination);



        Task<bool> IsEmailUniqueAsync(string? email, Guid currentUserId);
        Task<bool> IsUserNameUniqueAsync(string? userName, Guid currentUserId);
        Task<bool> IsNumberPhoneUniqueAsync(string? numberPhone, Guid currentUserId);

        Task<User?> GetByIdWithRoleAsync(Guid id, CancellationToken ct = default);

    }
}
