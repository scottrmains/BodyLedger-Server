using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class datechanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.RenameColumn(
                name: "scheduled_day",
                schema: "public",
                table: "template_assignments",
                newName: "cycle_day_offset");

            migrationBuilder.AddColumn<int>(
                name: "cycle_start_offset",
                schema: "public",
                table: "template_checklists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cycle_start_offset",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.RenameColumn(
                name: "cycle_day_offset",
                schema: "public",
                table: "template_assignments",
                newName: "scheduled_day");

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                schema: "public",
                table: "template_checklists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
