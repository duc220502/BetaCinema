using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class DBServiceExtensions
    {
        public static IServiceCollection AddDatabaseAndRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration["DatabaseProvider"];

            if (provider == "SqlServer")
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

                //services.AddScoped<IUserRepository, SqlServerUserRepository>();
            }
            else
            {
                throw new NotSupportedException($"Unsupported Database Provider: {provider}");
            }

            return services;
        }
    }
}
