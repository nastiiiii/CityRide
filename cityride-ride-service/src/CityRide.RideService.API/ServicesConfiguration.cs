using FluentValidation.AspNetCore;
using CityRide.RideService.Application.Services.Interfaces;
using CityRide.RideService.Domain.Repositories;
using CityRide.RideService.Infrastructure;
using CityRide.RideService.API.Ride.Requests;
using CityRide.RideService.Application.Profiles;
using CityRide.RideService.Application.Services;
using CityRide.Kafka.Interfaces;
using CityRide.Kafka;
using StackExchange.Redis;
using RedLockNet.SERedis.Configuration;
using System.Net;
using RedLockNet;
using RedLockNet.SERedis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CityRide.Events;
using Microsoft.EntityFrameworkCore;

namespace CityRide.RideService.API;

public static class ServicesConfiguration
{
    private static IConfiguration _configuration;

    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static void AddAppService(this IServiceCollection services)
    {
        services.AddSignalR();
        var sqliteConnectionString = _configuration.GetConnectionString("RideServiceContextDb") ?? string.Empty;
        services.AddDbContext<RideServiceContext>(opt => opt.UseSqlite(sqliteConnectionString), ServiceLifetime.Singleton);

        services.AddScoped<IRideRepository, RideRepository>();

        services.AddScoped<IRideService, Application.Services.RideService>();
        services.AddScoped<IRedisClientService, RedisClientService>();
        services.AddScoped<IRideRequestsService, RideRequestsService>();
        services.AddScoped<IDriverApiService, DriverApiService>();

        services.AddAutoMapper(typeof(DtoToRideResponse), typeof(RideToDto));
        services.AddFluentValidationAutoValidation();

        services.AddSingleton<IConsumerFactory<string, RideRequested>, ConsumerFactory<string, RideRequested>>();
        services.AddSingleton<IProducerFactory<string, RideStatusUpdated>, ProducerFactory<string, RideStatusUpdated>>();

        var multiplexer = ConnectionMultiplexer.Connect(_configuration["Redis:Host"]!);
        var endPoints = new List<RedLockEndPoint>
        {
            new DnsEndPoint(_configuration["Redis:Host"]!, int.Parse(_configuration["Redis:Port"]!))
        };
        services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        services.AddSingleton<IDistributedLockFactory, RedLockFactory>(x => RedLockFactory.Create(endPoints));
    }

    public static void AddJwtAuthentication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var encodedSecret = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _configuration["JWT:ValidIssuer"],
                ValidAudience = _configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(encodedSecret)
            };
        });
    }
}