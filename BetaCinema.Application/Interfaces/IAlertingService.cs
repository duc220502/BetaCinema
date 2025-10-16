using BetaCinema.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface IAlertingService
    {
        Task SendAdminAlertAsync(JobFailedAlertDto viewModel);

        Task SendReconciliationFailedAlertAsync(ReconciliationFailedAlertViewModel viewModel);

        Task SendSecurityAlertAsync(SecurityAlertViewModel viewModel);
    }
}
