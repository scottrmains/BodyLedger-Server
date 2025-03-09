using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class iteminheritance3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_completed",
                schema: "public",
                table: "workout_exercise_assignments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_completed",
                schema: "public",
                table: "workout_exercise_assignments",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
