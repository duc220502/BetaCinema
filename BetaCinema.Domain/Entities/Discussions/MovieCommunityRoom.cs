using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Discussions
{
    public class MovieCommunityRoom : BaseEntity
    {
        public bool IsActive { get; set; }

        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; } = default!;

        public virtual ICollection<MovieCommunityMember> Members { get; set; } = new List<MovieCommunityMember>();
        public virtual ICollection<MovieCommunityMessage> Messages { get; set; } = new List<MovieCommunityMessage>();


    }
}
