using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Notifications;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Shared.Pagination;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class UserNotificationService(IUserNotificationRepository userNotificationRepository ,
        IUnitOfWork unitOfWork , IOptions<NotificationSetting> options , ICurrentUserservice currentUserservice) : IUserNotificationService
    {
        private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;
        private readonly  IUnitOfWork _unitOfWork = unitOfWork;
        private readonly NotificationSetting _setting = options.Value;
        private readonly ICurrentUserservice _currentUserservice = currentUserservice;

        public async Task<ResponseObject<NotificationResponseDto>> GetUserNotificationsAsync( CancellationToken ct = default)
        {
            var userId = _currentUserservice.GetRequiredUserId();
 
            var pagination = new Pagination
            {
                PageNumber = 1,
                PageSize = _setting.DispatchBatchSize,
            };


            var pagedResult = await _userNotificationRepository.GetNotificationByUsersAsync(userId, pagination, ct);
            var unreadCount = await _userNotificationRepository.CountUnreadAsync(userId, ct);

            var dto = new NotificationResponseDto
            {
                UnreadCount = unreadCount,
                Items = pagedResult.Data.Select(x => new NotificationItemDto
                {
                    UserNotificationId = x.Id,
                    NotificationId = x.NotificationId,
                    Title = x.Notification!.Title,
                    Content = x.Notification.Content,
                    ActionUrl = x.Notification.ActionUrl,
                    ImgUrl = x.Notification.ImgUrl,
                    IsRead = x.IsRead,
                    CreateAt = x.Notification.CreatedAt,
                    DispatchStatus = x.Notification.DispatchStatus.ToString()
                }).ToList()
            };

            return ResponseObject<NotificationResponseDto>.ResponseSuccess("Lấy danh sách thông báo thành công.",dto);

        }

        public async Task<ResponseObject<int>> GetUnreadCountAsync( CancellationToken ct = default)
        {
            var userId = _currentUserservice.GetRequiredUserId();

            var unreadCount = await _userNotificationRepository.CountUnreadAsync(userId, ct);

            return ResponseObject<int>.ResponseSuccess("Lấy số lượng thông báo chưa đọc thành công.",unreadCount);
        }

        public async Task<ResponseObject<NotificationItemDto>> MarkAsReadAsync( Guid userNotificationId, CancellationToken ct = default)
        {

            if (userNotificationId == Guid.Empty)
                throw new BadRequestAppException("UserNotificationId không hợp lệ.");

            var entity = await _userNotificationRepository.GetByIdAsync(userNotificationId, ct)
                         ?? throw new NotFoundAppException("Không tìm thấy thông báo và user.");

            if (entity.IsDeleted)
                throw new BadRequestAppException("Thông báo này đã bị xóa.");

            if (!entity.IsRead)
            {
                entity.IsRead = true;
                entity.ReadAt = DateTime.UtcNow;

                _userNotificationRepository.Update(entity);
                await _unitOfWork.SaveChangesAsync(ct);
            }

            var dto = new NotificationItemDto
            {
                UserNotificationId = entity.Id,
                NotificationId = entity.NotificationId,
                Title = entity.Notification?.Title ?? string.Empty,
                Content = entity.Notification?.Content ?? string.Empty,
                ActionUrl = entity.Notification?.ActionUrl,
                ImgUrl = entity.Notification?.ImgUrl,
                IsRead = entity.IsRead,
                CreateAt = entity.Notification?.CreatedAt ?? DateTime.UtcNow,
                DispatchStatus = entity.Notification?.DispatchStatus.ToString()??string.Empty
                
            };

            return ResponseObject<NotificationItemDto>.ResponseSuccess("Đánh dấu đã đọc thành công.",dto);
        }
    }
}
