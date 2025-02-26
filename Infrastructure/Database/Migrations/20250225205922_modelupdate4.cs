using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class modelupdate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_assignments_template_checklists_template_checklist",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.DropTable(
                name: "template_checklists",
                schema: "public");

            migrationBuilder.RenameColumn(
                name: "cycle_day_offset",
                schema: "public",
                table: "template_assignments",
                newName: "time_of_day");

            migrationBuilder.AddColumn<int>(
                name: "scheduled_day",
                schema: "public",
                table: "template_assignments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "weekly_checklists",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    start_day = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_weekly_checklists", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_template_assignments_weekly_checklists_template_checklist_id",
                schema: "public",
                table: "template_assignments",
                column: "template_checklist_id",
                principalSchema: "public",
                principalTable: "weekly_checklists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_assignments_weekly_checklists_template_checklist_id",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.DropTable(
                name: "weekly_checklists",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "scheduled_day",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.RenameColumn(
                name: "time_of_day",
                schema: "public",
                table: "template_assignments",
                newName: "cycle_day_offset");

            migrationBuilder.CreateTable(
                name: "template_checklists",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    cycle_length_days = table.Column<int>(type: "integer", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_day = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template_checklists", x => x.id);
                });

            migrationBuilder.AddForeignKey(
                name: "fk_template_assignments_template_checklists_template_checklist",
                schema: "public",
                table: "template_assignments",
                column: "template_checklist_id",
                principalSchema: "public",
                principalTable: "template_checklists",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
