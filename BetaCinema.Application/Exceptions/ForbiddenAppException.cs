using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public sealed class ForbiddenAppException : AppException
    {
        public ForbiddenAppException( string message, object? meta = null)
             : base("FORBIDDEN", message, meta) { }
    }
}
