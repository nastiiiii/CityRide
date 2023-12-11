using CityRide.RideService.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using CityRide.RideService.Domain.Entities;
using CityRide.RideService.Infrastructure;
using CityRide.Domain.Enums;
using CityRide.RideService.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ServicesConfiguration.Initialize(builder.Configuration);
builder.Services.AddAppService();
builder.Services.AddJwtAuthentication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<RideRequestsHostedService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var rideServiceContext = scope.ServiceProvider.GetRequiredService<RideServiceContext>();
    // Check if the database exists
    var databaseExists = rideServiceContext.Database.GetService<IRelationalDatabaseCreator>().Exists();

    // If the database does not exist, create it and perform migrations
    if (!databaseExists)
        try
        {
            rideServiceContext.Database.EnsureCreated(); // Create the database
            rideServiceContext.Database.Migrate(); // Apply any pending migrations
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration has failed: {ex.Message}");
        }

    var testRide = rideServiceContext.Rides.FirstOrDefault(b => b.RideId == 1);
    if (testRide == null)
        rideServiceContext.Rides.Add(new Ride
        {
            From = new Location { Latitude = 4, Longitude = 6 },
            To = new Location { Latitude = 5, Longitude = 1 },
            ClientId = 4,
            DriverId = 5,
            Status = RideStatus.Accepted,
            Price = 5.3m
        });

    rideServiceContext.SaveChanges();
}

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<RideHub>("RideHub");
app.MapControllers();

app.Run();