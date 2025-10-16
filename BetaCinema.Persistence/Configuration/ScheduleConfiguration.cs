using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.ShowTimes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
    {
        public void Configure(EntityTypeBuilder<Schedule> builder)
        {
            builder.HasOne(x => x.Movie)
              .WithMany(x=>x.Schedules)
              .HasForeignKey(x => x.MovieId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Room)
              .WithMany(x => x.Schedules)
              .HasForeignKey(x => x.RoomId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => new { x.RoomId, x.StartAt })
              .IsUnique();
        }
    }
}
