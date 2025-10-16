using BetaCinema.Shared.Pagination;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Extensions
{
    public static class PaginationExtensions
    {
        public static async Task<PageResult<T>> ToPagedListAsync<T>(this IQueryable<T> source,Pagination pagination)
        {
            var totalCount = await source.CountAsync();

            var data = await source
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            pagination.TotalCount = totalCount;

            return new PageResult<T>(pagination, data);
        }
    }
}
