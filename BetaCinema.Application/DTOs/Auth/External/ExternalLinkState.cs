using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Auth.External
{
    public class ExternalLinkState
    {
        public string Provider { get; set; } = default!;
        public string ProviderKey { get; set; } = default!;
        public string Email { get; set; } = default!;
        public Guid UserId { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }


}
