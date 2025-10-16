using BetaCinema.Application.Interfaces;
using Hangfire;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Infrastructure
{
    public class HangfireJobService(IBackgroundJobClient hangfireClient) : IBackgroundJobService
    {
        private readonly IBackgroundJobClient _hangfireClient = hangfireClient;
        public void Enqueue<T>(Expression<Action<T>> job)
        {
            _hangfireClient.Enqueue(job);
        }
    }
}
