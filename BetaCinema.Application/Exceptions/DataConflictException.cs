using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public class DataConflictException : Exception
    {
        public DataConflictException(string message) : base(message) { }
    }
}
