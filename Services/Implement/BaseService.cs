using BetaCinema.DataContext;
using BetaCinema.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BetaCinema.Services.Implement
{
    public class BaseService
    {
        protected readonly AppDbContext _context;


        public BaseService()
        {
            _context = new AppDbContext();
        }

        protected string GennerateCode(string NameDefault)
        {
            return NameDefault + DateTime.Now.Ticks.ToString();
        }

        protected IQueryable<T> Result<T>(Pagination pagination, IQueryable<T> list)
        {
            IQueryable<T> result = PageResult<T>.ToPageResult(pagination, list);
            return result;
        }

    }
}
