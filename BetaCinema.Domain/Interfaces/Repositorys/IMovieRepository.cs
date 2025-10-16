using BetaCinema.Domain.Entities.ShowTimes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IMovieRepository : IRepository<Movie>
    {
        public Task<bool> IsNameUniqueAsync(string? name, Guid? id = null);
    }
}
