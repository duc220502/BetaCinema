using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IUserNotificationService
    {
        Task<ResponseObject<NotificationResponseDto>> GetUserNotificationsAsync( CancellationToken ct = default);
        Task<ResponseObject<int>> GetUnreadCountAsync( CancellationToken ct = default);
        Task<ResponseObject<NotificationItemDto>> MarkAsReadAsync(Guid userNotificationId , CancellationToken ct = default);
    }
}
