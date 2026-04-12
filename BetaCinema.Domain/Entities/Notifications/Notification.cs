using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Notifications
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public NotificationType  NotificationType { get; set; }

        public string? ActionUrl { get; set; }

        public string? ImgUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public NotificationAudienceType AudienceType { get; set; }

        public NotificationDispatchStatus DispatchStatus { get; set; }

        public DateTime?  ExpireAt { get; set; }

        public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();


    }
}
