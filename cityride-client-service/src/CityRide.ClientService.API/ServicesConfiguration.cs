using System.Text;
using CityRide.ClientService.API.Client.Requests;
using CityRide.Events;
using CityRide.Kafka;
using CityRide.Kafka.Interfaces;
using CityRide.ClientService.API.Authentication.Requests;
using CityRide.ClientService.API.Profiles;
using CityRide.ClientService.API.Validators;
using CityRide.ClientService.Application.Profiles;
using CityRide.ClientService.Application.Services;
using CityRide.ClientService.Application.Services.Interfaces;
using CityRide.ClientService.Domain.Repositories;
using CityRide.ClientService.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CityRide.ClientService.API
{
    public static class ServicesConfiguration
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddDbContext<ClientServiceContext>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IClientService, Application.Services.ClientService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IBillingApiService, BillingApiService>();
            services.AddAutoMapper(typeof(CreateClientRequestToDto), typeof(ClientToDto));
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateClientRequest>, CreateClientRequestValidator>();
            services.AddScoped<IValidator<UpdateClientRequest>, UpdateClientRequestValidator>();
            services.AddScoped<IValidator<LogInRequest>, LogInRequestValidator>();

            services.AddSingleton<IProducerFactory<string, RideRequested>, ProducerFactory<string, RideRequested>>();
            services.AddSingleton<IConsumerFactory<string, RideStatusUpdated>, ConsumerFactory<string, RideStatusUpdated>>();
        }

        public static void AddSwagger(this IServiceCollection serviceCollection)
        {
            
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("V1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Client Service",
                    Description = "Client Service Web Api"
                });
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
        
        public static void AddJWTAuthentication(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var encodedSecret = Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? string.Empty);

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
}
