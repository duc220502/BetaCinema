using BetaCinema.Application.UseCases;
using BetaCinema.Domain.Entities.Discussions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class MovieReviewConfiguration : IEntityTypeConfiguration<MovieReview>
    {
        public void Configure(EntityTypeBuilder<MovieReview> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status)
           .HasConversion<int>()
           .IsRequired();

            builder.HasOne(x => x.Movie)
              .WithMany(x => x.MovieReviews)
              .HasForeignKey(x => x.MovieId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
             .WithMany(x => x.MovieReviews)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(x => new { x.MovieId, x.Status, x.CreateAt });

            builder.HasIndex(x => new { x.UserId, x.Status });

            builder.HasIndex(x => new { x.UserId, x.MovieId })
                .IsUnique()
                .HasFilter("[DeleteAt] IS NULL");

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_MovieReviews_Rating", "[Rating] IS NULL OR [Rating] BETWEEN 1 AND 10");

            });

        }
    }
}
