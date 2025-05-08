using BetaCinema.Entities;
using BetaCinema.Handle;
using BetaCinema.PayLoads.Convertest;
using BetaCinema.PayLoads.DataResponses;
using BetaCinema.PayLoads.Responses;
using BetaCinema.Services.Implement;
using BetaCinema.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace BetaCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Làm theo mẫu này.Vd Beare {Token}",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                x.SchemaGeneratorOptions.UseInlineDefinitionsForEnums = true;

            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Secretkey").Value))
                };

            });

            var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();

            builder.Services.AddSingleton(emailConfig); 

            builder.Services.AddScoped<IEmailService,EmailService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserSevice, UserService>();
            builder.Services.AddScoped<ICinemaService, CinemaService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IFoodService, FoodService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IMovieService, MovieService>();

            builder.Services.AddScoped<MovieService>();

            builder.Services.AddScoped<IScheduleService,ScheduleService>();
            builder.Services.AddScoped<IBillService, BillService>();
            builder.Services.AddScoped<ITicketService, TicketService>();

            builder.Services.AddScoped<IVnpayService, VnpayService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
