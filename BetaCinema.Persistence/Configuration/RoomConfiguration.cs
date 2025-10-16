using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class RoomConfiguration: IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasOne(x => x.Cinema)
              .WithMany(x=>x.Rooms)
              .HasForeignKey(x => x.CinemaId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
