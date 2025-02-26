using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class modelupdate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cycle_start_offset",
                schema: "public",
                table: "template_checklists",
                newName: "start_day");

            migrationBuilder.AddColumn<bool>(
                name: "is_recurring",
                schema: "public",
                table: "template_assignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_recurring",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.RenameColumn(
                name: "start_day",
                schema: "public",
                table: "template_checklists",
                newName: "cycle_start_offset");
        }
    }
}
