using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Notifications
{
    public class UserNotification : BaseEntity
    {

        public bool IsRead { get; set; }

        public DateTime? ReadAt { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeleteAt { get; set; }
        public Guid UserId { get; set; }
        public virtual User?  User { get; set; }

        public Guid NotificationId { get; set; }
        public virtual Notification? Notification { get; set; }

       

    }
}
