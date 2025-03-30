using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class workoutsets2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assignment_items_assignments_template_assignment_id",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.RenameColumn(
                name: "template_assignment_id",
                schema: "public",
                table: "assignment_items",
                newName: "assignment_id");

            migrationBuilder.RenameIndex(
                name: "ix_assignment_items_template_assignment_id",
                schema: "public",
                table: "assignment_items",
                newName: "ix_assignment_items_assignment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_assignment_items_assignments_assignment_id",
                schema: "public",
                table: "assignment_items",
                column: "assignment_id",
                principalSchema: "public",
                principalTable: "assignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_assignment_items_assignments_assignment_id",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.RenameColumn(
                name: "assignment_id",
                schema: "public",
                table: "assignment_items",
                newName: "template_assignment_id");

            migrationBuilder.RenameIndex(
                name: "ix_assignment_items_assignment_id",
                schema: "public",
                table: "assignment_items",
                newName: "ix_assignment_items_template_assignment_id");

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
    }
}
