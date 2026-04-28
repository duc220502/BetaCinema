using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Discussions
{
    public class MovieCommunityMember : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }

        public DateTime JoinedAt { get; set; }
        public DateTime? LastSeenAt { get; set; }
        public bool IsMuted { get; set; }

        public virtual MovieCommunityRoom MovieCommunityRoom { get; set; } = default!;
        public virtual User User { get; set; } = default!;
    }
}
