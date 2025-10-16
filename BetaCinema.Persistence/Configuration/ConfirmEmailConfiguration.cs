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
    public class ConfirmEmailConfiguration : IEntityTypeConfiguration<ConfirmEmail>
    {
        public void Configure(EntityTypeBuilder<ConfirmEmail> builder)
        {
            builder.HasOne(x => x.User)
              .WithMany(x=>x.ConfirmEmails)
              .HasForeignKey(x => x.UserId)
              .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
