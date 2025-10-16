using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Repository
{
    public class PriceRepository(AppDbContext context) : BaseRepository<PriceTier>(context), IPriceRepository
    {

        public async Task<PriceTier?> FindBestMatchingRuleAsync(DateTime scheduleStartAt)
        {
            var scheduleDate = DateOnly.FromDateTime(scheduleStartAt);
            var timeOfDay = TimeOnly.FromDateTime(scheduleStartAt);
            var dayOfWeekBitmask = 1 << (int)scheduleStartAt.DayOfWeek;

            var applicableRule = await _context.PriceTiers
                .Include(pt => pt.PriceRuleType) 
                .Where(pt =>
                    pt.StartAt <= timeOfDay && pt.EndAt >= timeOfDay &&
                    (
                        pt.ApplicableDate == scheduleDate ||


                        (pt.ApplicableDay.HasValue && (pt.ApplicableDay & dayOfWeekBitmask) > 0)
                    )
                )
                .OrderByDescending(pt => pt.PriceRuleType!.PriorityValue) 
                .FirstOrDefaultAsync();

            return applicableRule;
        }
    }
}
