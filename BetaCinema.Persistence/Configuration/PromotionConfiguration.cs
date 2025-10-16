using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
    {
        public void Configure(EntityTypeBuilder<Promotion> builder)
        {
            builder.HasOne(x => x.RankCustomer)
              .WithMany(x=>x.Promotions)
              .HasForeignKey(x => x.RankCustomerId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
