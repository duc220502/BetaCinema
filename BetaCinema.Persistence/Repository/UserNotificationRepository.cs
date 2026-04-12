using BetaCinema.Domain.Entities.Notifications;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
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
    public class UserNotificationRepository(AppDbContext context) : BaseRepository<UserNotification>(context), IUserNotificationRepository
    {
        public void AddRange(IEnumerable<UserNotification> entities)
        => _context.UserNotifications.AddRange(entities);

        public Task<int> CountUnreadAsync(Guid userId, CancellationToken ct = default)
        => _context.UserNotifications
           .CountAsync(x => x.UserId == userId && !x.IsDeleted && !x.IsRead, ct);

        public Task<UserNotification?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => _context.UserNotifications.Include(x=>x.Notification).FirstOrDefaultAsync(x => x.Id == id, ct);

        public  Task<PageResult<UserNotification>> GetNotificationByUsersAsync(Guid userId, Pagination pagination, CancellationToken ct = default)
        {
            var now = DateTime.UtcNow;

            var query = _context.UserNotifications
                .AsNoTracking()
                .Include(x => x.Notification)
                .Where(x =>
                    x.UserId == userId &&
                    !x.IsDeleted &&
                    x.Notification != null &&
                    (x.Notification.ExpireAt == null || x.Notification.ExpireAt > now))
                .OrderByDescending(x => x.Notification!.CreatedAt);

            return  query.ToPagedListAsync(pagination);
        }
        

        public Task<List<UserNotification>> GetTopByUserAsync(Guid userId, int take, CancellationToken ct = default)
        => _context.UserNotifications.Include(x => x.Notification)
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.Notification!.CreatedAt)
            .Take(take)
            .ToListAsync(ct);
    }
}
