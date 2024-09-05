using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salonytics.Migrations
{
    /// <inheritdoc />
    public partial class addratingTip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppointmentId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Ratings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TipAmount",
                table: "Appointments",
                type: "real",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_AppointmentId",
                table: "Ratings",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "AppointmentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Appointments_AppointmentId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_AppointmentId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "TipAmount",
                table: "Appointments");
        }
    }
}
