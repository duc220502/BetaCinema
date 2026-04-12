using BetaCinema.Domain.Entities.Notifications;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IUserNotificationRepository : IRepository<UserNotification>
    {
        Task<PageResult<UserNotification>> GetNotificationByUsersAsync( Guid userId, Pagination pagination,CancellationToken ct = default);
        Task<List<UserNotification>> GetTopByUserAsync(Guid userId, int take, CancellationToken ct = default);
        Task<int> CountUnreadAsync(Guid userId, CancellationToken ct = default);
        Task<UserNotification?> GetByIdAsync(Guid id, CancellationToken ct = default);
        void AddRange(IEnumerable<UserNotification> entities);

    }
}
