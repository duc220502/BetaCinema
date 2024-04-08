using BetaCinema.Entities;
using Microsoft.EntityFrameworkCore;

namespace BetaCinema.DataContext
{
    public class AppDbContext:DbContext
    {
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillFood> BillFoods { get; set; }
        public virtual DbSet<BillStatus> BillStatuses { get; set; }
        public virtual DbSet<BillTicket> BillTickets { get; set; }
        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<ConfirmEmail> ConfirmEmails { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieType> MovieTypes { get; set; }
        public virtual DbSet<Promotion> Promotions { get; set; }
        public virtual DbSet<RankCustomer> RankCustomers { get; set; }

        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Seat> Seats { get; set; }
        public virtual DbSet<SeatStatus> SeatStatuses { get; set; }
        public virtual DbSet<SeatType> SeatTypes { get; set; }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserStatus> UserStatuses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer($"server = ANHDUC\\ANHDUC;Database = Cinema;Trusted_Connection=true;TrustServerCertificate=True;");
        }
    }
}
