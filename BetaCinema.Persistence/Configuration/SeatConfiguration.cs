using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Seats;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasOne(x => x.Room)
              .WithMany(x=>x.Seats)
              .HasForeignKey(x => x.RoomId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SeatStatus)
              .WithMany(x => x.Seats)
              .HasForeignKey(x => x.SeatStatusId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.SeatType)
              .WithMany(x => x.Seats)
              .HasForeignKey(x => x.SeatTypeId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
