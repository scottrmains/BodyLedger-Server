using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class loadsachanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assignment_items_template_assignments_template_assignment_id",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.DropTable(
                name: "template_assignments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "weekly_checklists",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "checklists",
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
                    table.PrimaryKey("pk_checklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scheduled_day = table.Column<int>(type: "integer", nullable: false),
                    time_of_day = table.Column<int>(type: "integer", nullable: false),
                    completed = table.Column<bool>(type: "boolean", nullable: false),
                    completed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    checklist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false),
                    recurring_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_assignments_checklists_checklist_id",
                        column: x => x.checklist_id,
                        principalSchema: "public",
                        principalTable: "checklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_assignments_templates_template_id",
                        column: x => x.template_id,
                        principalSchema: "public",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_assignments_checklist_id",
                schema: "public",
                table: "assignments",
                column: "checklist_id");

            migrationBuilder.CreateIndex(
                name: "ix_assignments_template_id",
                schema: "public",
                table: "assignments",
                column: "template_id");

            migrationBuilder.AddForeignKey(
                name: "fk_assignment_items_assignments_template_assignment_id",
                schema: "public",
                table: "assignment_items",
                column: "template_assignment_id",
                principalSchema: "public",
                principalTable: "assignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assignment_items_assignments_template_assignment_id",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.DropTable(
                name: "assignments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "checklists",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "weekly_checklists",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    start_day = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_weekly_checklists", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "template_assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_checklist_id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed = table.Column<bool>(type: "boolean", nullable: false),
                    completed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_recurring = table.Column<bool>(type: "boolean", nullable: false),
                    recurring_start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    scheduled_day = table.Column<int>(type: "integer", nullable: false),
                    time_of_day = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_template_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_template_assignments_templates_template_id",
                        column: x => x.template_id,
                        principalSchema: "public",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_template_assignments_weekly_checklists_template_checklist_id",
                        column: x => x.template_checklist_id,
                        principalSchema: "public",
                        principalTable: "weekly_checklists",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_template_assignments_template_checklist_id",
                schema: "public",
                table: "template_assignments",
                column: "template_checklist_id");

            migrationBuilder.CreateIndex(
                name: "ix_template_assignments_template_id",
                schema: "public",
                table: "template_assignments",
                column: "template_id");

            migrationBuilder.AddForeignKey(
                name: "fk_assignment_items_template_assignments_template_assignment_id",
                schema: "public",
                table: "assignment_items",
                column: "template_assignment_id",
                principalSchema: "public",
                principalTable: "template_assignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
