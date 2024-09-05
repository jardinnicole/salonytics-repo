using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salonytics.Migrations
{
    /// <inheritdoc />
    public partial class hsa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_HomeServiceAvailabilities_BranchId",
                table: "HomeServiceAvailabilities",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_HomeServiceAvailabilities_StylistId",
                table: "HomeServiceAvailabilities",
                column: "StylistId");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeServiceAvailabilities_Branches_BranchId",
                table: "HomeServiceAvailabilities",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.NoAction);


            migrationBuilder.AddForeignKey(
                name: "FK_HomeServiceAvailabilities_Stylists_StylistId",
                table: "HomeServiceAvailabilities",
                column: "StylistId",
                principalTable: "Stylists",
                principalColumn: "StylistId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeServiceAvailabilities_Branches_BranchId",
                table: "HomeServiceAvailabilities");

            migrationBuilder.DropForeignKey(
                name: "FK_HomeServiceAvailabilities_Stylists_StylistId",
                table: "HomeServiceAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_HomeServiceAvailabilities_BranchId",
                table: "HomeServiceAvailabilities");

            migrationBuilder.DropIndex(
                name: "IX_HomeServiceAvailabilities_StylistId",
                table: "HomeServiceAvailabilities");
        }
    }
}
