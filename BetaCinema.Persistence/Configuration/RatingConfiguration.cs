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
    public class RatingConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedNever();

            builder.HasIndex(r => r.Code).IsUnique();

            builder.HasData(
               new Rate { Id = (int)Rating.P, Code = "P", Name = "Mọi lứa tuổi", Description = "Phim được phép phổ biến rộng rãi đến mọi đối tượng." },
               new Rate { Id = (int)Rating.C13, Code = "C13", Name = "Cấm trẻ em dưới 13 tuổi", Description = "Phim cấm khán giả dưới 13 tuổi." },
               new Rate { Id = (int)Rating.C16, Code = "C16", Name = "Cấm trẻ em dưới 16 tuổi", Description = "Phim cấm khán giả dưới 16 tuổi." },
               new Rate { Id = (int)Rating.C18, Code = "C18", Name = "Cấm trẻ em dưới 18 tuổi", Description = "Phim cấm khán giả dưới 18 tuổi." }
            );
        }
    }
}
