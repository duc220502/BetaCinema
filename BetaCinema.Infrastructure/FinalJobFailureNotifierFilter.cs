using BetaCinema.Application.DTOs;
using BetaCinema.Application.Interfaces;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure
{
    public class FinalJobFailureNotifierFilter(IServiceProvider serviceProvider): JobFilterAttribute, IApplyStateFilter
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            if (context.NewState is FailedState failedState)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var backgroundJobService = scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();
                    var alertingService = scope.ServiceProvider.GetRequiredService<IAlertingService>();

                    var jobName = context.BackgroundJob.Job.Method.Name;
                    var arguments = string.Join(", ", context.BackgroundJob.Job.Args.Select(a => a?.ToString() ?? "null"));
                    var exceptionMessage = failedState.Exception.ToString();

                    var viewModel = new JobFailedAlertDto
                    {
                        JobId = context.BackgroundJob.Id,
                        JobName = jobName,
                        Arguments = arguments,
                        ExceptionDetails = exceptionMessage,
                        FailedAt = DateTime.UtcNow
                    };

                    backgroundJobService.Enqueue<IAlertingService>(
                        service => service.SendAdminAlertAsync(viewModel));
                }
            }
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
           
        }
    }
}
