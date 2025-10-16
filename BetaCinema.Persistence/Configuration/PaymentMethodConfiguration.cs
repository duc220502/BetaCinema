using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    class PaymentMethodConfiguration : IEntityTypeConfiguration<Domain.Entities.Orders.PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Orders.PaymentMethod> builder)
        {
            builder.HasKey(pm => pm.Id);

            builder.Property(pm => pm.Id).ValueGeneratedNever();

            builder.Property(pm => pm.MethodCode);

            builder.HasIndex(pm => pm.MethodCode).IsUnique();

            builder.HasData(
            new PaymentMethod { Id = (int)Domain.Enums.PaymentMethod.CASH, MethodCode = "CASH", MethodName = "Thanh toán tại quầy" },
            new PaymentMethod { Id = (int)Domain.Enums.PaymentMethod.VNPAY, MethodCode = "VNPAY", MethodName = "Thanh toán qua VNPAY" },
            new PaymentMethod { Id = (int)Domain.Enums.PaymentMethod.MOMO, MethodCode = "MOMO", MethodName = "Thanh toán qua Ví MoMo" }
        );
        }

    }
}
