using BetaCinema.Domain.Entities.Users;
using BetaCinema.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class RankConfiguration : IEntityTypeConfiguration<RankCustomer>
    {
        public void Configure(EntityTypeBuilder<RankCustomer> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedNever();

            builder.HasIndex(r => r.RankName).IsUnique();

            builder.HasData(
               new RankCustomer { Id = (int)UserRank.Standard, RankName = "Standard", MinimumPoint = 0,IsActive = true },
               new RankCustomer { Id = (int)UserRank.Silver, RankName = "Silver", MinimumPoint = 200000 , IsActive = true },
               new RankCustomer { Id = (int)UserRank.Gold, RankName = "Gold", MinimumPoint = 500000 , IsActive = true },
               new RankCustomer { Id = (int)UserRank.Platinum, RankName = "Platinum", MinimumPoint = 1000000 , IsActive = true }
       );
        }
    }
}
