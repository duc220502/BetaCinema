using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IBackgroundJobService
    {
        void Enqueue<T>(Expression<Action<T>> job);
    }
}
