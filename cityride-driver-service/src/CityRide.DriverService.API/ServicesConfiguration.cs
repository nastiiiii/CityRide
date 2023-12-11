using System.Text;
using CityRide.DriverService.API.Authentication.Requests;
using CityRide.DriverService.API.Driver.Requests;
using CityRide.DriverService.API.Profiles;
using CityRide.DriverService.API.Validators;
using CityRide.DriverService.Application.Profiles;
using CityRide.DriverService.Application.Services;
using CityRide.DriverService.Application.Services.Interfaces;
using CityRide.DriverService.Domain.Repositories;
using CityRide.DriverService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CityRide.DriverService.API;

public static class ServicesConfiguration
{
    public static void AddAppServices(this IServiceCollection services)
    {
        services.AddScoped<IDriverRepository, DriverRepository>();
        services.AddScoped<IDriverLocationRepository, DriverLocationRepository>();
        services.AddScoped<IDriverService, Application.Services.DriverService>();
        services.AddScoped<IDriverLocationService, DriverLocationService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddAutoMapper(typeof(DriverLocationToDto), typeof(CreateDriverRequestToDto));
        services.AddDbContext<DriverServiceContext>();
        services.AddFluentValidationAutoValidation();
        services.AddScoped<IValidator<CreateDriverRequest>, CreateDriverRequestValidator>();
        services.AddScoped<IValidator<UpdateDriverRequest>, UpdateDriverRequestValidator>();
        services.AddScoped<IValidator<LogInRequest>, LogInRequestValidator>();

        services.AddSignalR();
    }

    public static void AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("V1", new OpenApiInfo
            {
                Version = "V1",
                Title = "Driver Service",
                Description = "Driver Service Web Api"
            }
            );

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Description = "Bearer Authentication with a JWT Token",
                Type = SecuritySchemeType.Http
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
    }

    public static void AddJwtAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            var encodedSecret = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]);

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = configuration["JWT:ValidIssuer"],
                ValidAudience = configuration["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(encodedSecret)
            };
        });
    }
}