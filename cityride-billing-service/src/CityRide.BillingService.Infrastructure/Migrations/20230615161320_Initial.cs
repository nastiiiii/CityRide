using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRide.billing_service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Coefficient = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RidePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceX = table.Column<double>(type: "REAL", nullable: false),
                    SourceY = table.Column<double>(type: "REAL", nullable: false),
                    DestinationX = table.Column<double>(type: "REAL", nullable: false),
                    DestinationY = table.Column<double>(type: "REAL", nullable: false),
                    CostPerKm = table.Column<double>(type: "REAL", nullable: false),
                    ExtraFees = table.Column<double>(type: "REAL", nullable: false),
                    CarClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RidePrices", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarClasses");

            migrationBuilder.DropTable(
                name: "RidePrices");
        }
    }
}
