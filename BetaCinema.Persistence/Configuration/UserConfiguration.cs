using BetaCinema.Domain.Entities.Orders;
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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasOne(x => x.RankCustomer)
              .WithMany(x=>x.Users)
              .HasForeignKey(x => x.RankCustomerId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Role)
              .WithMany(x => x.Users)
              .HasForeignKey(x => x.RoleId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.UserStatus)
              .WithMany(x => x.Users)
              .HasForeignKey(x => x.UserStatusId)
              .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(u => u.UserName).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.NumberPhone).IsUnique();

            builder.Ignore(u => u.IsActive);
        }
    }
}
