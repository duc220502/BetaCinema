using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Common
{
    public class NotificationSetting
    {
        public int DispatchBatchSize { get; set; } = 1000;
        public bool GlobalDispatchEnabled { get; set; } = true;
    }
}
