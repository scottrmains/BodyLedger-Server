using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class templatedbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_assignments_template_template_id",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.AddForeignKey(
                name: "fk_template_assignments_templates_template_id",
                schema: "public",
                table: "template_assignments",
                column: "template_id",
                principalSchema: "public",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_assignments_templates_template_id",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.AddForeignKey(
                name: "fk_template_assignments_template_template_id",
                schema: "public",
                table: "template_assignments",
                column: "template_id",
                principalSchema: "public",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
