using BetaCinema.Domain.Entities.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class CartFoodItemConfiguration : IEntityTypeConfiguration<CartFoodItem>
    {
        public void Configure(EntityTypeBuilder<CartFoodItem> builder)
        {
            builder.HasOne(x => x.Cart)
           .WithMany(x => x.CartFoodItems)
           .HasForeignKey(x => x.CartId)
           .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Food)
            .WithMany(x => x.CartFoodItems)
            .HasForeignKey(x => x.FoodId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.CartId);
            builder.HasIndex(x => x.FoodId);
        }
    }
}
