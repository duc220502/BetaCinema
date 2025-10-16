using BetaCinema.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class BillConfiguration : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasOne(x => x.BillStatus)
              .WithMany(x=>x.Bills)
              .HasForeignKey(x => x.BillStatusId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.User)
              .WithMany(x=>x.Bills)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.Ignore(x => x.IsActive);
        }
    }
}
