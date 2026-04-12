using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseObject<NotificationDto>> CreateUserNotificationAsync(Request_CreateNotification request, CancellationToken ct = default);


    }
}
