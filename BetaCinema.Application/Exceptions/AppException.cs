using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public string ErrorCode { get; }
        public object? Meta { get; }

        protected AppException(string errorCode, string message, object? meta = null)
            : base(message)
        {
            ErrorCode = errorCode;
            Meta = meta;
        }
    }
}
