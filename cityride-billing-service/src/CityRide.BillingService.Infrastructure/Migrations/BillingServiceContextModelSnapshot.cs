﻿// <auto-generated />
using CityRide.BillingService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CityRide.billing_service.Infrastructure.Migrations
{
    [DbContext(typeof(BillingServiceContext))]
    partial class BillingServiceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0-preview.5.23280.1");

            modelBuilder.Entity("CityRide.BillingService.Domain.Entities.RidePrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CarClass")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Coefficient")
                        .HasColumnType("REAL");

                    b.Property<double>("CostPerKm")
                        .HasColumnType("REAL");

                    b.Property<double>("ExtraFees")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("RidePrices");
                });
#pragma warning restore 612, 618
        }
    }
}
