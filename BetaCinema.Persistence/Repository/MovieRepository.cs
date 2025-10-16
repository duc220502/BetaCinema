using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class MovieRepository(AppDbContext context) : BaseRepository<Movie>(context) , IMovieRepository
    {
        public async Task<bool> IsNameUniqueAsync(string? name, Guid? id)
        {
            if (id == null)
                return !await _context.Movies.AnyAsync(x => x.Name == name && x.IsActive);

            return !await _context.Movies.AnyAsync(u => u.Name == name && u.IsActive && u.Id != id);
        }
    }
}
