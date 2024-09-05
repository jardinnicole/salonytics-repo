using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace salonytics.Migrations
{
    /// <inheritdoc />
    public partial class queueid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Queues",
                table: "Queues");

            // Modify the column
            migrationBuilder.AlterColumn<string>(
                name: "QueueId",
                table: "Queues",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Add the primary key constraint back
            migrationBuilder.AddPrimaryKey(
                name: "PK_Queues",
                table: "Queues",
                column: "QueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Queues",
                table: "Queues");

            // Revert the column modification
            migrationBuilder.AlterColumn<Guid>(
                name: "QueueId",
                table: "Queues",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            // Re-add the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_Queues",
                table: "Queues",
                column: "QueueId");
        }
    }
}
