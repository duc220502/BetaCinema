using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get ; set ; } = default!;
    }
}
