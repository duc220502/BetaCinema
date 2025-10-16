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
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>
    {
        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder.HasOne(x => x.MovieType)
              .WithMany(x=>x.Movies)
              .HasForeignKey(x => x.MovieTypeId)
              .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Rate)
              .WithMany(x=>x.Movies)
              .HasForeignKey(x => x.RateId)
              .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
