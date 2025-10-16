using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string resourceName, object key)
            : base($"Resource '{resourceName}' ({key}) was not found.") { }
    }
}
