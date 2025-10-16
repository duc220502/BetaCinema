using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.DTOs
{
    public class JobFailedAlertDto
    {
        public string JobId { get; init; } = default!;
        public string JobName { get; init; } = default!;
        public string Arguments { get; init; } = default!;
        public string ExceptionDetails { get; init; } = default!;
        public DateTime FailedAt { get; init; }
    }
}
