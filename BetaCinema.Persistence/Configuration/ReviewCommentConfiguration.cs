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
    public class ReviewCommentConfiguration : IEntityTypeConfiguration<ReviewComment>
    {
        public void Configure(EntityTypeBuilder<ReviewComment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Depth)
            .IsRequired();

            builder.Property(x => x.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(x => x.Review)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.User)
             .WithMany(x => x.ReviewComments)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MentionUser)
                .WithMany(x => x.MentionedInReviewComments)
                .HasForeignKey(x => x.MentionUserId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(x => x.ParentComment)
           .WithMany(x => x.ChildComments)
           .HasForeignKey(x => x.ParentCommentId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.RootComment)
           .WithMany(x => x.RootComments)
           .HasForeignKey(x => x.RootCommentId)
           .OnDelete(DeleteBehavior.Restrict);



            builder.HasOne(x => x.ReplyToComment)
            .WithMany(x=>x.ReplyReferences)
            .HasForeignKey(x => x.ReplyToCommentId)
            .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(x => new { x.ReviewId, x.Depth, x.Status, x.CreatedAt });

            builder.HasIndex(x => new { x.RootCommentId, x.Status, x.CreatedAt });

            builder.HasIndex(x => new { x.ParentCommentId, x.Status, x.CreatedAt });

            builder.HasIndex(x => new { x.ReplyToCommentId });

            builder.HasIndex(x => new { x.UserId, x.CreatedAt });


            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_ReviewComments_Depth", "[Depth] IN (0,1,2)");
                t.HasCheckConstraint(
                    "CK_ReviewComments_Depth_Relation",
                    "([Depth] = 0 AND [ParentCommentId] IS NULL AND [RootCommentId] IS NULL AND [ReplyToCommentId] IS NULL) " +
                    "OR " +
                    "([Depth] IN (1,2) AND [ParentCommentId] IS NOT NULL AND [RootCommentId] IS NOT NULL AND [ReplyToCommentId] IS NOT NULL)"
                );
            });
        }
    }
}
