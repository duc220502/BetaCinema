using BetaCinema.Application.Interfaces.Catching;
using BetaCinema.Infrastructure.Catching.Redis;
using BetaCinema.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
          


            services.AddOptions<RedisSettings>()
                .Bind(configuration.GetSection("Redis"))
                .ValidateDataAnnotations()
                .Validate(cfg => !string.IsNullOrWhiteSpace(cfg.Connection), "Redis:Connection is required")
                .Validate(cfg => !string.IsNullOrWhiteSpace(cfg.Instance), "Redis:Instance is required")
                .ValidateOnStart();


            services.AddScoped<IAppCache, AppCache>();
            services.AddScoped<IIdempotencyService,IdempotencyService>();
            services.AddScoped<IRateLimiter, RateLimiter>();    
            services.AddScoped<ISeatHoldService, SeatHoldService>();

            // 2) ConnectionMultiplexer (singleton) – duy nhất cho toàn app
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var rc = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

                var options = ConfigurationOptions.Parse(rc.Connection);
                options.AbortOnConnectFail = false;                    // không kill app nếu Redis tạm time-out
                options.ConnectRetry = rc.MaxRetry;
                options.ConnectTimeout = rc.ConnectTimeoutMs;
                options.ReconnectRetryPolicy = new ExponentialRetry(  // retry kết nối mượt hơn
                    deltaBackOffMilliseconds: 2000);

                // Bạn có thể set thêm:
                // options.KeepAlive = 15; options.ConfigCheckSeconds = 60; options.ClientName = "betacinema-api";

                return ConnectionMultiplexer.Connect(options);
            });
            // 3) IDistributedCache dùng Redis (cache/session…)
            services.AddStackExchangeRedisCache(o =>
            {
                // NOTE: IDistributedCache không nhận IOptions<T> trực tiếp → lấy từ configuration
                var rc = configuration.GetSection("Redis").Get<RedisSettings>()!;
                o.Configuration = rc.Connection;
                o.InstanceName = rc.Instance; // prefix key theo môi trường
            });

            return services;
        }
    }
}
