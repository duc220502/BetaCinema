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
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(x => x.Schedule)
              .WithMany(x=>x.Tickets)
              .HasForeignKey(x => x.ScheduleId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Seat)
              .WithMany(x=>x.Tickets)
              .HasForeignKey(x => x.SeatId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
