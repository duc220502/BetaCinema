using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public sealed class BadRequestAppException : AppException
    {
        public BadRequestAppException( string message, object? meta = null)
            : base("BAD_REQUEST", message, meta) { }
    }
}
