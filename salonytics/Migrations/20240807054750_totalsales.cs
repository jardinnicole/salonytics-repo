using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salonytics.Migrations
{
    /// <inheritdoc />
    public partial class totalsales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalSales",
                table: "Stylists");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalSales",
                table: "Stylists",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
