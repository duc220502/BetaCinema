using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Application.Interfaces.Catching;
using BetaCinema.Application.Interfaces.PaymentStrategies;
using BetaCinema.Application.Mapping;
using BetaCinema.Infrastructure.Authentication;
using BetaCinema.Infrastructure.Catching.Redis;
using BetaCinema.Infrastructure.Configuration;
using BetaCinema.Infrastructure.Emails;
using BetaCinema.Infrastructure.Payments;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure.Extensions
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserservice, CurrentUserService>();
            services.AddScoped<IPasswordSecurityService, BCryptSecuriryService>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IConfirmationMethodStrategy, OtpConfirmationStrategy>();
            services.AddScoped<IConfirmationMethodStrategy, LinkConfirmationStrategy>();
            services.AddScoped<IAlertingService, EmailAlertingService>();
            services.AddScoped<IRankUpNotificationService , EmailRankUpNotificationService>();
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.Configure<VnpayConfig>(configuration.GetSection("Vnpay"));
            services.Configure<AlertingSettings>(configuration.GetSection("AlertingSettings"));
            services.AddScoped<IBackgroundJobService, HangfireJobService>();
            services.AddScoped<IPaymentStrategy , CashPaymentStrategy>();
            services.AddScoped<IPaymentStrategy,VnpayPaymentStrategy>();
            services.AddScoped<IPaymentStrategyFactory , PaymentStrategyFactory>();
            services.AddScoped<IVnpayClient, VnpayClient>();


            services.AddScoped<ISeatHoldService, SeatHoldService>();

            services.AddScoped<IRazorTemplateService, RazorTemplateService>();
            services.AddTransient<FinalJobFailureNotifierFilter>();
            services.AddHttpClient();
            services.AddHangfire((serviceProvider, globalConfiguration) =>
            {

                var connectionString = configuration.GetConnectionString("SqlServer");
                globalConfiguration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString);


                globalConfiguration.UseFilter(serviceProvider.GetRequiredService<FinalJobFailureNotifierFilter>());
            });




            services.AddHangfireServer();


           

            services.AddAutoMapper(typeof(UserProfile).Assembly);
            return services;
        }
    }
}
