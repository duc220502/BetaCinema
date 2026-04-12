using BetaCinema.Domain.Entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {

            builder.HasOne(x => x.User)
                .WithMany(x => x.UserNotifications)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Notification)
                .WithMany(x => x.UserNotifications)
                .HasForeignKey(x => x.NotificationId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.UserId, x.IsRead });
            builder.HasIndex(x => new { x.UserId, x.NotificationId })
                .IsUnique();
        }
    }
}
