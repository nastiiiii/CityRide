using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRide.billing_service.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCarClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarClasses");

            migrationBuilder.DropColumn(
                name: "CarClassId",
                table: "RidePrices");

            migrationBuilder.DropColumn(
                name: "DestinationX",
                table: "RidePrices");

            migrationBuilder.DropColumn(
                name: "DestinationY",
                table: "RidePrices");

            migrationBuilder.DropColumn(
                name: "SourceX",
                table: "RidePrices");

            migrationBuilder.RenameColumn(
                name: "SourceY",
                table: "RidePrices",
                newName: "Coefficient");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RidePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "RidePrices");

            migrationBuilder.RenameColumn(
                name: "Coefficient",
                table: "RidePrices",
                newName: "SourceY");

            migrationBuilder.AddColumn<int>(
                name: "CarClassId",
                table: "RidePrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DestinationX",
                table: "RidePrices",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DestinationY",
                table: "RidePrices",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SourceX",
                table: "RidePrices",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "CarClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Coefficient = table.Column<double>(type: "REAL", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarClasses", x => x.Id);
                });
        }
    }
}
