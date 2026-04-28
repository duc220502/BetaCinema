using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Discussions
{
    public class MovieReview : BaseEntity
    {
        public string Title { get; set; } = default!;

        public string Content { get; set; } = default!;

        public int? Rating { get; set; }



        public bool IsSpoiler { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public int LikeCount { get; set; }
        public int LoveCount { get; set; }
        public int HahaCount { get; set; }
        public int WowCount { get; set; }
        public int SadCount { get; set; }
        public int AngryCount { get; set; }
        public int ReactionCount { get; set; }

        public int CommentCount { get; set; }



        public ReviewStatus Status { get; set; } = default!;

        public DateTime CreateAt { get; set; }

        public DateTime  UpdateAt { get; set; }

        public DateTime? DeleteAt { get; set; }

        public Guid MovieId { get; set; }
        public virtual Movie? Movie { get; set; } 


        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public virtual ICollection<ReviewComment> Comments { get; set; } = new List<ReviewComment>();

    }
}
