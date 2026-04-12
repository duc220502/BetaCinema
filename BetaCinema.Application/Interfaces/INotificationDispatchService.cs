using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Interfaces
{
    public interface INotificationDispatchService
    {
        public Task DispatchGlobalNotificationAsync(Guid notificationId, CancellationToken ct = default);

        public Task DispatchToUsersAsync(Guid notificationId, List<Guid> userIds, CancellationToken ct = default);
    }
}
