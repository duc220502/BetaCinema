using BetaCinema.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Configuration
{
    public class BillStatusConfiguration : IEntityTypeConfiguration<BillStatus>
    {
        public void Configure(EntityTypeBuilder<BillStatus> builder)
        {
          

            builder.Property(bs => bs.Id).ValueGeneratedNever();

            builder.Property(pm => pm.Code);

            builder.HasIndex(pm => pm.Code).IsUnique();


            builder.HasData(
               new BillStatus { Id = (int)Domain.Enums.BillStatus.PendingPayment,Code = "PendingPayment", StatusName = "Đang chờ thanh toán" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.Paid,Code = "Paid", StatusName = "Đã thanh toán" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.CancelledByUser,Code = "CancelledByUser", StatusName = "Đã hủy bởi người dùng" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.Expired , Code = "Expired", StatusName = "Đã hết hạn" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.Failed , Code = "Failed", StatusName = "Thanh toán thất bại" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.Refunded , Code = "Refunded", StatusName = "Đã hoàn tiền" },
               new BillStatus { Id = (int)Domain.Enums.BillStatus.ReconciliationFailed, Code = "ReconciliationFailed", StatusName = "Đối soát thất bại" }
       );
        }
    }
}
