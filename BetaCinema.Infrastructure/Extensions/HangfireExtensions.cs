using BetaCinema.Application.Interfaces;
using BetaCinema.Application.UseCases;
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
    public static class HangfireExtensions
    {

        public static void  AddHangfireJobs(this IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            var cleanupIntervalInMinutes = configuration.GetValue<int>("ScheduleSettings:CleaningBufferMinutes");

            if (cleanupIntervalInMinutes <= 0)
            {
                cleanupIntervalInMinutes = 1; 
            }

            var recurringJobManager = serviceProvider.GetRequiredService<IRecurringJobManager>();

            recurringJobManager.AddOrUpdate<IBillCleanUpService>(
                  recurringJobId: "cleanup-expired-bills",
                  methodCall: service => service.ProcessExpiredBillsAsync(),
                  cronExpression: $"*/{cleanupIntervalInMinutes} * * * *"
              );
        }
    }
}
