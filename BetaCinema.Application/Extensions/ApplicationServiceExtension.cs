using BetaCinema.Application.Common;
using BetaCinema.Application.Interfaces;
using BetaCinema.Application.Interfaces.AI;
using BetaCinema.Application.Interfaces.Auths;
using BetaCinema.Application.Interfaces.Catching;
using BetaCinema.Application.UseCases;
using BetaCinema.Application.UseCases.AI;
using BetaCinema.Application.UseCases.Auths;
using BetaCinema.Application.UseCases.Users;
using BetaCinema.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaCinema.Application.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();
            services.AddScoped<IConfirmationCodeService,ConfirmationCodeService>();
            services.AddScoped<IConfirmationEmailService,ConfirmationEmailService>();
            services.AddScoped<ICinemaService, CinemaService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IUserPromotionService, UserPromotionService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IBillService, BillService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBillCleanUpService , BillCleanupService>();
            services.AddScoped<IPaymentService , PaymentService>();
            services.AddScoped<IExternalAuthService, ExternalAuthService>();
            services.AddScoped<IExternalIdentityNormalizer, ExternalIdentityNormalizer>();
            services.AddScoped<IExternalLinkingService, ExternalLinkingService>();


            services.AddScoped<IAiChatService, AiChatService>();
            services.AddScoped<IAiContextResolver, AiModelClientResolver>();
            services.AddScoped<IAiPromptBuilder, CinemaPromptBuilder>();


            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<INotificationDispatchService, NotificationDispatchService>();
            services.AddScoped<ICartService, CartService>();



            services.Configure<ScheduleSettings>(configuration.GetSection("ScheduleSettings"));
            services.Configure<NotificationSetting>(configuration.GetSection("NotificationSettings"));
            services.Configure<GeminiOptions>(configuration.GetSection("Gemini"));

            return services;
        }
    }
}
