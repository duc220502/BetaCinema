
using BetaCinema.API.Extensions;
using BetaCinema.API.Filters;
using BetaCinema.Application.Common;
using BetaCinema.Application.Extensions;
using BetaCinema.Infrastructure.Extensions;
using BetaCinema.Persistence.DBContext;
using BetaCinema.Persistence.Extensions;
using Hangfire;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;

namespace BetaCinema.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDatabaseAndRepositories(builder.Configuration);
            builder.Services.AddPersistenceServices();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new NullableDateTimeConverter());
            });
            builder.Services.AddApiExtension();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddFluentValidation();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddBffCookie(builder.Configuration);
            builder.Services.AddExternalProviders(builder.Configuration);
            builder.Services.AddJWTAuthentication(builder.Configuration);
            builder.Services.AddCustomAuthorization();
            var app = builder.Build();

            app.UseHangfireDashboard();

            app.Services.AddHangfireJobs();

           // app.UseCustomMiddlewares();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
            });

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();


            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;


            app.Run();
        }
    }
}
