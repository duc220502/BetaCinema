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
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ReactionType)
           .HasConversion<int>()
           .IsRequired();

            builder.HasOne(x => x.User)
           .WithMany()
           .HasForeignKey(x => x.UserId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Review)
                .WithMany(x => x.Reactions)
                .HasForeignKey(x => x.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(x => x.Comment)
           .WithMany(x => x.Reactions)
           .HasForeignKey(x => x.CommentId)
           .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.UserId, x.ReviewId })
                .IsUnique()
                .HasFilter("[ReviewId] IS NOT NULL");

            builder.HasIndex(x => new { x.UserId, x.CommentId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL");

            builder.HasIndex(x => new { x.ReviewId, x.ReactionType });
            builder.HasIndex(x => new { x.CommentId, x.ReactionType });


            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Reactions_Target_OnlyOne",
                    "(([ReviewId] IS NOT NULL AND [CommentId] IS NULL) OR ([ReviewId] IS NULL AND [CommentId] IS NOT NULL))"
                );
            });
        }
    }
}
