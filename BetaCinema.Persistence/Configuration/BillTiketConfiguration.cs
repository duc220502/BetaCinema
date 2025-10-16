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
    public class BillTiketConfiguration : IEntityTypeConfiguration<BillTicket>
    {
        public void Configure(EntityTypeBuilder<BillTicket> builder)
        {
            builder.HasOne(x => x.Bill)
              .WithMany(x=>x.BillTickets)
              .HasForeignKey(x => x.BillId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Ticket)
              .WithMany(x=>x.BillTickets)
              .HasForeignKey(x => x.TicketId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
