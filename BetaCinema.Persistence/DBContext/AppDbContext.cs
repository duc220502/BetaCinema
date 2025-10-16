using BetaCinema.Domain.Entities;
using BetaCinema.Domain.Entities.Foods;
using BetaCinema.Domain.Entities.Orders;
using BetaCinema.Domain.Entities.Promotions;
using BetaCinema.Domain.Entities.Seats;
using BetaCinema.Domain.Entities.ShowTimes;
using BetaCinema.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillFood> BillFoods { get; set; }

        public DbSet<BillPromotion> BillPromotions { get; set; }

        public DbSet<BillStatus> BillStatuses { get; set; }

        public DbSet<BillTicket> BillTickets { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<ConfirmEmail> ConfirmEmails { get; set; }

        public DbSet<Food> Foods { get; set; }

        public DbSet<GeneralSetting> GeneralSettings { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<MovieType> MovieTypes { get; set; }

        public DbSet<Promotion> Promotions { get; set; }

        public DbSet<RankCustomer> RankCustomers { get; set; }

        public DbSet<Rate> Rates { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<SeatStatus> SeatStatuses { get; set; }
        public DbSet<SeatType> SeatTypes { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserPromotion> UserPromotions { get; set; }

        public DbSet<UserStatus> UserStatuses { get; set; }

        public DbSet<PriceTier> PriceTiers { get; set; }

        public DbSet<PriceRuleType> PriceRuleTypes { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<ExternalLogin> ExternalLogins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
