using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salonytics.Migrations
{
    /// <inheritdoc />
    public partial class StylistSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StylistSchedules",
                table: "StylistSchedules");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StylistSchedules");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
                table: "StylistSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_StylistSchedules",
                table: "StylistSchedules",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_StylistSchedules",
                table: "StylistSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "StylistSchedules");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StylistSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StylistSchedules",
                table: "StylistSchedules",
                column: "Id");
        }
    }
}
