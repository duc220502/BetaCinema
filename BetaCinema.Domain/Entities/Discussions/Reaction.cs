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
    public class Reaction : BaseEntity
    {
        public ReactionType ReactionType { get; set; }

        public Guid? ReviewId { get; set; }
        public virtual MovieReview? Review { get; set; }

        public Guid? CommentId { get; set; }
        public virtual ReviewComment? Comment { get; set; }

        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

    }
}
