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
    public class BaseRepository<T>:IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private readonly DbSet<T> _set;
        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public async  Task<T?> GetByIdAsync(Guid ?id , CancellationToken ct = default)
            => await _set.FindAsync(id, ct);

        public void Update(T entity) => _set.Update(entity);

        public void Delete(T entity) => _set.Remove(entity);

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
       => _context.SaveChangesAsync(ct);

        public void Add(T entity) =>_set.Add(entity);
        
    }
}
