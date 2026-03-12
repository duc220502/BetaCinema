using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public sealed class ConflictAppException : AppException
    {
        public ConflictAppException(string message, object? meta = null)
            : base("CONFLICT", message, meta) { }
    }
}
