using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public sealed class NotFoundAppException : AppException
    {
        public NotFoundAppException(string message, object? meta = null)
        : base("NOT_FOUND", message, meta) { }
    }
}
