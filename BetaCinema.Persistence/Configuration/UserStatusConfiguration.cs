using BetaCinema.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class UserStatusConfiguration : IEntityTypeConfiguration<UserStatus>
    {
        public void Configure(EntityTypeBuilder<UserStatus> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedNever();

            builder.HasIndex(r => r.StatusName).IsUnique();

            builder.HasData(
                new UserStatus { Id = (int)Domain.Enums.UserStatus.PendingActivation, StatusName = "Chờ kích hoạt" },
                new UserStatus { Id = (int)Domain.Enums.UserStatus.Active, StatusName = "Đang hoạt động" },
                new UserStatus { Id = (int)Domain.Enums.UserStatus.Locked, StatusName = "Đã khóa" },
                new UserStatus { Id = (int)Domain.Enums.UserStatus.Banned, StatusName = "Đã cấm" },
                new UserStatus { Id = (int)Domain.Enums.UserStatus.Deactivated, StatusName = "Đã vô hiệu hóa" }
            );
        }
    }
}
