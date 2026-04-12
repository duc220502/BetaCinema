using BetaCinema.Application.DTOs.Notification;
using BetaCinema.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Validators.Notification
{
    public class AddNotificationValidator : AbstractValidator<Request_CreateNotification>
    {
        public AddNotificationValidator()
        {
            RuleFor(x => x)
            .Must(x =>
                x.AudienceType != NotificationAudienceType.Global ||
                x.UserIds == null || !x.UserIds.Any())
            .WithMessage("Notification global không được truyền UserIds.");

            RuleFor(x => x)
                .Must(x =>
                    x.AudienceType != NotificationAudienceType.SelectedUsers ||
                    (x.UserIds != null && x.UserIds.Any()))
                .WithMessage("Notification gửi theo danh sách user phải có UserIds.");
        }
    }
}
