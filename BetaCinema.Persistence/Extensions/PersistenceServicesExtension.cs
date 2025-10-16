using BetaCinema.Application.Interfaces;
using BetaCinema.Domain.Interfaces.Repositorys;
using BetaCinema.Persistence.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Persistence.Extensions
{
    public static class PersistenceServicesExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
           
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserStatusRepository, UserStatusRepository>();
            services.AddScoped<IRankCustomerRepository, RankCustomerRepository>();
            services.AddScoped<IConfirmEmailRepository, ConfirmEmailRepository>();
            services.AddScoped<ICinemaRepository, CinemaRepository>();
            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<ISeatStatusRepository, SeatStatusRepository>();
            services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IRateRepository, RateRepository>();
            services.AddScoped<IMovieTypeRepository, MovieTypeRepository>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            services.AddScoped<IUserPromotionRepository, UserPromotionRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IPriceRepository, PriceRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillStatusRepository, BillStatusRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IExternalLoginRepository , ExternalLoginRepository>();
            services.AddMemoryCache();


            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
