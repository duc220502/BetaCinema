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
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Id).ValueGeneratedNever();

            builder.HasIndex(r=>r.RoleName).IsUnique();

            builder.HasData(
               new Role { Id = (int)UserRole.Admin, RoleName = "Admin" },
               new Role { Id = (int)UserRole.Staff, RoleName = "Staff" },
               new Role { Id = (int)UserRole.Member, RoleName = "Member" }
       );
        }
    }
}
