using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Domain.Enums
{
    public enum PriceRuleType
    {
        GeneralWeekday = 1,
        GeneralWeekend = 2,
        GoldenHour = 3,
        HappyTuesday = 4,
        HolidayEvent = 5
    }
}
