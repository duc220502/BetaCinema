using BetaCinema.API.Filters;
using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using Polly;

namespace BetaCinema.API.Extensions
{
    public static class APIExtensions
    {
        public static IServiceCollection AddApiExtension(this IServiceCollection services )
        {
            services.AddScoped<TransactionFilter>();
            services.AddHttpClient("VNPAY", c =>
            {
                c.BaseAddress = new Uri("https://sandbox.vnpayment.vn");
                c.Timeout = TimeSpan.FromSeconds(10);
            })
           .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(new[]
           {
                TimeSpan.FromMilliseconds(200),
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(800),
           }));
            return services;
        }
    }
}
