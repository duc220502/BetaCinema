using BetaCinema.Application.Common;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Notifications;
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
    public class NotificationDispatchService(IOptions<NotificationSetting> options , IUserRepository userRepository , 
        IUserNotificationRepository userNotificationRepository,IUnitOfWork unitOfWork) : INotificationDispatchService
    {
        private readonly NotificationSetting _setting = options.Value;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserNotificationRepository _userNotificationRepository = userNotificationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task DispatchGlobalNotificationAsync(Guid notificationId, CancellationToken ct = default)
        {
            var pageNumber = 1;
            while (true)
            {
                var pagination = new Pagination
                {
                    PageNumber = pageNumber,
                    PageSize = _setting.DispatchBatchSize,
                };


                var users = (await _userRepository.GetPagedActiveUserIdsAsync(pagination, ct)).Data;
                if (!users.Any()) break;

                var rows = users.Select(user => new UserNotification
                {
                    UserId = user.Id,
                    NotificationId = notificationId,
                    IsRead = false,
                    IsDeleted = false
                }).ToList();

                _userNotificationRepository.AddRange(rows);
                await _unitOfWork.SaveChangesAsync(ct);

                pagination.PageNumber++;
            }

        }

        public async Task DispatchToUsersAsync(Guid notificationId, List<Guid> userIds, CancellationToken ct = default)
        {
            if (userIds == null || !userIds.Any())
                return;

            var distinctUserIds = userIds.Where(x => x != Guid.Empty).Distinct().ToList();



            foreach (var batch in distinctUserIds.Chunk(_setting.DispatchBatchSize))
            {
                ct.ThrowIfCancellationRequested();

                var validUserIds = await _userRepository.GetActiveUserIdsInAsync(batch.ToList(), ct);
                if (!validUserIds.Any())
                    continue;

                var rows = validUserIds.Select(userId => new UserNotification
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    NotificationId = notificationId,
                    IsRead = false,
                    IsDeleted = false,
                    ReadAt = null,
                    DeleteAt = null
                }).ToList();

                _userNotificationRepository.AddRange(rows);
                await _unitOfWork.SaveChangesAsync(ct);
            }
        }
    }
    
}
