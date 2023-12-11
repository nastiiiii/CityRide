using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRide.billing_service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCarClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarClass",
                table: "RidePrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarClass",
                table: "RidePrices");
        }
    }
}
