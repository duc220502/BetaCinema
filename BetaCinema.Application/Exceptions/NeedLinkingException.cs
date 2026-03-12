using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public class NeedLinkingException : AppException
    {
        public NeedLinkingException(string message, object? meta = null) : base("ACCOUNT_LINKING_REQUIRED", message, meta) { }
        
    }
}
