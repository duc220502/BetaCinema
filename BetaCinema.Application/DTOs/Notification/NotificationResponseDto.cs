using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Notification
{
    public class NotificationResponseDto
    {
        public int UnreadCount { get; set; }
        public List<NotificationItemDto> Items { get; set; } = [];
    }
}
