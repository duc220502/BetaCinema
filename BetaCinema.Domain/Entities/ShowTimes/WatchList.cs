using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.ShowTimes
{
    public class WatchList : BaseEntity
    {
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public Guid MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }

}
