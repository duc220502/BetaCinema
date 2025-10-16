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
    public class SeatTypeConfiguration : IEntityTypeConfiguration<SeatType>
    {
        public void Configure(EntityTypeBuilder<SeatType> builder)
        {
            builder.Property(st => st.Id).ValueGeneratedOnAdd();

            builder.HasData(
            new SeatType { Id = (int)Domain.Enums.SeatType.Standard, NameType = "Standard", Surcharge = 0m },
            new SeatType { Id = (int)Domain.Enums.SeatType.VIP, NameType = "VIP", Surcharge = 20000m },
            new SeatType { Id = (int)Domain.Enums.SeatType.Sweetbox, NameType = "Sweetbox", Surcharge = 50000m }
        );
        }
    }
}
