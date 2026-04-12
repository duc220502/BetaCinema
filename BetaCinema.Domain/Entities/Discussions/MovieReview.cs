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

        public int Rating { get; set; }

        public bool IsSpoiler { get; set; }

        public bool IsVerifiedPurchase { get; set; }

        public int CommentCount { get; set; }

        public int LikeCount { get; set; }

        public ReviewStatus Status { get; set; } = default!;

        public DateTime CreateAt { get; set; }

        public DateTime  UpdateAt { get; set; }



    }
}
