using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class checklistLogs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_checklist_logs_checklist_id",
                schema: "public",
                table: "checklist_logs");

            migrationBuilder.CreateIndex(
                name: "ix_checklist_logs_checklist_id",
                schema: "public",
                table: "checklist_logs",
                column: "checklist_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_checklist_logs_checklist_id",
                schema: "public",
                table: "checklist_logs");

            migrationBuilder.CreateIndex(
                name: "ix_checklist_logs_checklist_id",
                schema: "public",
                table: "checklist_logs",
                column: "checklist_id");
        }
    }
}
