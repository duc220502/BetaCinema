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
    public class WatchListConfiguration : IEntityTypeConfiguration<WatchList>
    {
        public void Configure(EntityTypeBuilder<WatchList> builder)
        {
            builder.HasOne(x => x.User)
                .WithMany(x => x.WatchLists)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Movie)
                .WithMany(x => x.WatchLists)
                .HasForeignKey(x => x.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.MovieId);

            builder.HasIndex(x => new { x.UserId, x.MovieId })
                .IsUnique()
                .HasDatabaseName("UX_Watchlists_UserId_MovieId");
        }
    }
}
