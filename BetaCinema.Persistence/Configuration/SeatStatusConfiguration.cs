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
    public class SeatStatusConfiguration : IEntityTypeConfiguration<SeatStatus>
    {
         public void Configure(EntityTypeBuilder<SeatStatus> builder) 
         {
            builder.Property(ss => ss.Id).ValueGeneratedOnAdd();

            builder.HasData(
            new SeatStatus { Id = (int)Domain.Enums.SeatStatus.Available, Code = "AVAILABLE", NameStatus = "Còn trống" },
            new SeatStatus { Id = (int)Domain.Enums.SeatStatus.Held, Code = "HELD", NameStatus = "Đang giữ" },
            new SeatStatus { Id = (int)Domain.Enums.SeatStatus.Booked, Code = "BOOKED", NameStatus = "Đã bán" }
         );

        }
    }
}
