using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Discussions
{
    public class MovieCommunityMessage : BaseEntity
    {
        public Guid RoomId { get; set; }
        public virtual MovieCommunityRoom? Room { get; set; } 
        public Guid UserId { get; set; }
        public virtual User? User { get; set; };

        public Guid? ParentMessageId { get; set; }
        public virtual MovieCommunityMessage? ParentMessage { get; set; }
        public Guid? MentionUserId { get; set; }
        public virtual User? MentionUser { get; set; }

        public string Content { get; set; } = default!;
        public bool IsEdited { get; set; }
        public bool IsActive { get; set; }

        public int ReactionCount { get; set; }

        public virtual ICollection<MovieCommunityMessage> Replies { get; set; } = new List<MovieCommunityMessage>();

        public virtual ICollection<MovieCommunityMessageReaction> Reactions { get; set; } = new List<MovieCommunityMessageReaction>();

    }
}
