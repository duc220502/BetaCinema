using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Discussions
{
    public class ReviewComment : BaseEntity
    {


        public string Content { get; set; } = string.Empty;

        public bool IsEdited { get; set; }
        public CommentStatus Status { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int LikeCount { get; set; }
        public int LoveCount { get; set; }
        public int HahaCount { get; set; }
        public int WowCount { get; set; }
        public int SadCount { get; set; }
        public int AngryCount { get; set; }
        public int ReactionCount { get; set; }
        public int ReplyCount { get; set; }


        public int Depth { get; set; } = 1;

        public Guid? RootCommentId { get; set; }
        public virtual ReviewComment? RootComment { get; set; }


        public Guid? ParentCommentId { get; set; }
        public virtual ReviewComment? ParentComment { get; set; }



        public Guid? ReplyToCommentId { get; set; }
        public virtual ReviewComment? ReplyToComment { get; set; }

        public Guid? MentionUserId { get; set; }
        public virtual User? MentionUser { get; set; }


        public Guid ReviewId { get; set; }
        public virtual MovieReview? Review { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<ReviewComment> ChildComments { get; set; } = new List<ReviewComment>();
        public virtual ICollection<ReviewComment> ReplyReferences { get; set; } = new List<ReviewComment>();
        public virtual ICollection<ReviewComment> RootComments { get; set; } = new List<ReviewComment>();
        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();


    }
}
