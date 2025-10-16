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
    public class UserPromotionConfiguration : IEntityTypeConfiguration<UserPromotion>
    {
        public void Configure(EntityTypeBuilder<UserPromotion> builder)
        {
            builder.HasIndex(x => new { x.UserId, x.PromotionId }).IsUnique();

            builder.HasOne(x => x.Promotion)
              .WithMany(x=>x.UserPromotions)
              .HasForeignKey(x => x.PromotionId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
              .WithMany(x=>x.UserPromotions)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
