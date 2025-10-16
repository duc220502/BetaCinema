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
    public class BillFoodConfiguration : IEntityTypeConfiguration<BillFood>
    {
        public void Configure(EntityTypeBuilder<BillFood> builder)
        {
            builder.HasOne(x => x.Bill)
              .WithMany(x=>x.BillFoods)
              .HasForeignKey(x => x.BillId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Food)
              .WithMany(x=>x.BillFoods)
              .HasForeignKey(x => x.FoodId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
