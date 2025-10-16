using BetaCinema.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class PriceRuleTypeConfiguration : IEntityTypeConfiguration<PriceRuleType>
    {
        public void Configure(EntityTypeBuilder<PriceRuleType> builder)
        {
            builder.Property(prt => prt.Id).ValueGeneratedNever();

            builder.HasIndex(pr => pr.Name).IsUnique();

            builder.HasData(
               new PriceRuleType { Id = (int)Domain.Enums.PriceRuleType.GeneralWeekday, Name = "Quy tắc chung Ngày thường", PriorityValue = 0 },
               new PriceRuleType { Id = (int)Domain.Enums.PriceRuleType.GeneralWeekend, Name = "Quy tắc chung Cuối tuần", PriorityValue = 0 },
               new PriceRuleType { Id = (int)Domain.Enums.PriceRuleType.GoldenHour, Name = "Khung Giờ Vàng", PriorityValue = 10 },
               new PriceRuleType { Id = (int)Domain.Enums.PriceRuleType.HappyTuesday, Name = "Thứ Ba Vui Vẻ", PriorityValue = 20 },
               new PriceRuleType { Id = (int)Domain.Enums.PriceRuleType.HolidayEvent, Name = "Sự kiện/Ngày Lễ", PriorityValue = 100 }
            );
        }
    }
}
