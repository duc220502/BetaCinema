using BetaCinema.Domain.Entities.Notifications;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class NotificationRepository(AppDbContext context) : BaseRepository<Notification>(context),INotificationRepository
    {
    }
}
