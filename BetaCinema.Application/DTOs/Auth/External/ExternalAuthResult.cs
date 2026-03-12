using BetaCinema.Application.DTOS.DataResponse;
using BetaCinema.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs.Auth.External
{
    public class ExternalAuthResult
    {
        public bool RequiresLinking { get; set; }
        public string? LinkingToken { get; set; }
        public string? Email { get; set; }
        public string? Provider { get; set; }

        public User? User { get; set; } = default!;
    }
}
