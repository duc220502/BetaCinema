using BetaCinema.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class BillPromotionConfiguration : IEntityTypeConfiguration<BillPromotion>
    {
        public void Configure(EntityTypeBuilder<BillPromotion> builder)
        {
            builder.HasIndex(bp => new { bp.BillId, bp.PromotionId }).IsUnique();

            builder.HasOne(x => x.Promotion)
            .WithMany(x=>x.BillPromotions)
            .HasForeignKey(x => x.PromotionId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Bill)
              .WithMany(x=>x.BillPromotions)
              .HasForeignKey(x => x.BillId)
              .OnDelete(DeleteBehavior.NoAction);
        }


    }
}
