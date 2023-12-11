using CityRide.Domain.Enums;
using CityRide.DriverService.API;
using CityRide.DriverService.Domain.Entities;
using CityRide.DriverService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAppServices();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddControllers().AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});;
builder.Services.AddSwagger();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/V1/swagger.json", "DriverApp"); });

using (var scope = app.Services.CreateScope())
{
    var appContext = scope.ServiceProvider.GetRequiredService<DriverServiceContext>();
    if (!appContext.Database.GetService<IRelationalDatabaseCreator>().Exists())
        try
        {
            await appContext.Database.EnsureCreatedAsync();
            appContext.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Migration has failed: {ex.Message}");
        }

    var testDriver = appContext.Drivers.FirstOrDefault(d => d.Id == 1);
    if (testDriver == null)
        appContext.Drivers.Add(new Driver
        {
            Email = "driver1@mail.com",
            Password = "zZZ3Y1Ekiz0chJOmvJpo6qPquGpH6SMa+7dRz65Uj00=",
            FirstName = "James",
            LastName = "Bond",
            PhoneNumber = "1234567890",
            CarClass = CarClass.Comfort
        });

        appContext.Drivers.Add(new Driver
        {
            Email = "driver2@mail.com",
            Password = "zZZ3Y1Ekiz0chJOmvJpo6qPquGpH6SMa+7dRz65Uj00=",
            FirstName = "James",
            LastName = "Bond",
            PhoneNumber = "1234567890",
            CarClass = CarClass.Comfort
        });

        appContext.Drivers.Add(new Driver
        {
            Email = "driver3@mail.com",
            Password = "zZZ3Y1Ekiz0chJOmvJpo6qPquGpH6SMa+7dRz65Uj00=",
            FirstName = "James",
            LastName = "Bond",
            PhoneNumber = "1234567890",
            CarClass = CarClass.Comfort
        });
    var driverLocation = appContext.DriverLocations.FirstOrDefault(d => d.Id == 1);
    if (driverLocation == null)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(4326);
        appContext.DriverLocations.Add(new DriverLocation
        {
            DriverId = 1,
            Id = 1,
            Location = geometryFactory.CreatePoint(new Coordinate(49.8348, 24.00763)),
            Status = DriverStatus.Available
        });
        appContext.DriverLocations.Add(new DriverLocation
        {
            DriverId = 2,
            Id = 2,
            Location = geometryFactory.CreatePoint(new Coordinate(49.83671, 24.00615)),
            Status = DriverStatus.Available
        });
        appContext.DriverLocations.Add(new DriverLocation
        {
            DriverId = 3,
            Id = 3,
            Location = geometryFactory.CreatePoint(new Coordinate(49.83118, 24.01411)),
            Status = DriverStatus.Available
        });
    }

    appContext.SaveChanges();
}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();