using BetaCinema.DataContext;
using BetaCinema.Entities;
using BetaCinema.Handle;

namespace BetaCinema.Services.Implements
{
    public class BaseService
    {
        protected readonly AppDbContext _context;

        public BaseService()
        {
            _context = new AppDbContext();
        }

        protected IQueryable<T> Result<T>(Pagination pagination, IQueryable<T> list)
        {
            IQueryable<T> result = PageResult<T>.ToPageResult(pagination, list);
            return result;
        }
    }
}
