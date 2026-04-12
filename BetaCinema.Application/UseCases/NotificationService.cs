using AutoMapper;
using BetaCinema.Application.Common;
using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Application.Exceptions;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Entities.Notifications;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces.Repositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.UseCases
{
    public class NotificationService(INotificationRepository notificationRepository  ,IUnitOfWork unitOfWork , IMapper mapper , IBackgroundJobService backgroundJobService ) : INotificationService
    {
        private readonly INotificationRepository _notificationRepository = notificationRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IBackgroundJobService _backgroundJobService = backgroundJobService;


        public async Task<ResponseObject<NotificationDto>> CreateUserNotificationAsync(Request_CreateNotification request, CancellationToken ct = default)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                NotificationType = request.NotificationType,
                ActionUrl = request.ActionUrl,
                ImgUrl = request.ImgUrl,
                CreatedAt = DateTime.UtcNow,
                AudienceType = request.AudienceType,
                ExpireAt = request.ExpireAt,
                DispatchStatus = NotificationDispatchStatus.Pending,
                
            };



            _notificationRepository.Add(notification);
            await _unitOfWork.SaveChangesAsync();

            if (request.AudienceType == Domain.Enums.NotificationAudienceType.Global)
            {

                _backgroundJobService.Enqueue<INotificationDispatchService>(sevice => sevice.DispatchGlobalNotificationAsync(notification.Id, ct));
                
                return ResponseObject<NotificationDto>.ResponseSuccess("Thêm notification thành công" , _mapper.Map<NotificationDto>(notification));
            }


            _backgroundJobService.Enqueue<INotificationDispatchService>(
                service => service.DispatchToUsersAsync(notification.Id,request.UserIds!,ct));

            return ResponseObject<NotificationDto>.ResponseSuccess("Thêm notification thành công", _mapper.Map<NotificationDto>(notification));

        }

       
    }
}
