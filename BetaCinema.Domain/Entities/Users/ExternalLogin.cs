using BetaCinema.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Entities.Users
{
    public class ExternalLogin : BaseEntity
    {
        public Guid UserId { get; set; }          
        public User User { get; set; } = default!;

        public string Provider { get; set; } = default!;
        public string ProviderKey { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
