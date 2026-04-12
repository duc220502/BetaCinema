using BetaCinema.Domain.Enums;
using BetaCinema.Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Notification
{
    public class Request_CreateNotification
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? ActionUrl { get; set; }
        public string? ImgUrl { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime? ExpireAt { get; set; }

        public NotificationAudienceType AudienceType { get; set; }

        public List<Guid>? UserIds { get; set; }
    }
}
