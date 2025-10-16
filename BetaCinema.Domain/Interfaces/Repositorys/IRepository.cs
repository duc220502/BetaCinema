using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Interfaces.Repositorys
{
    public interface IRepository<T> 
    {
        Task<T?> GetByIdAsync(Guid ?id, CancellationToken ct = default);


        public void Update(T entity);
        public void Delete(T entity);

        public void Add(T entity);  
        public Task<int> SaveChangesAsync(CancellationToken ct = default);

    }
}
