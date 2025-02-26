using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class removedateupdated2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "start_day",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                schema: "public",
                table: "template_checklists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.AddColumn<int>(
                name: "start_day",
                schema: "public",
                table: "template_checklists",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
