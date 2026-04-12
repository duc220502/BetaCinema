using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Notification
{
    public class NotificationDto
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? ActionUrl { get; set; }
        public string? ImgUrl { get; set; }
        public DateTime CreateAt { get; set; }
        public string DispatchStatus { get; set; } = default!;

        public string NotificationType { get; set; } = default!;
    }
}
