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
    public class MovieCommunityMessageReaction : BaseEntity
    {
        public Guid MessageId { get; set; }
        public virtual MovieCommunityMessage? Message { get; set; } 

        public Guid UserId { get; set; }
        public virtual User? User { get; set; } 
        public ReactionType ReactionType { get; set; }

        
        
    }
}
