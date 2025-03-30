using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class workoutsets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "actual_weight",
                schema: "public",
                table: "workout_activity_assignments");

            migrationBuilder.DropColumn(
                name: "completed_reps",
                schema: "public",
                table: "workout_activity_assignments");

            migrationBuilder.DropColumn(
                name: "completed_sets",
                schema: "public",
                table: "workout_activity_assignments");

            migrationBuilder.CreateTable(
                name: "workout_sets",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_activity_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    set_number = table.Column<int>(type: "integer", nullable: false),
                    reps = table.Column<int>(type: "integer", nullable: false),
                    weight = table.Column<int>(type: "integer", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workout_sets", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_sets_workout_activity_assignments_workout_activity_",
                        column: x => x.workout_activity_assignment_id,
                        principalSchema: "public",
                        principalTable: "workout_activity_assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_workout_sets_workout_activity_assignment_id",
                schema: "public",
                table: "workout_sets",
                column: "workout_activity_assignment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workout_sets",
                schema: "public");

            migrationBuilder.AddColumn<int>(
                name: "actual_weight",
                schema: "public",
                table: "workout_activity_assignments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "completed_reps",
                schema: "public",
                table: "workout_activity_assignments",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "completed_sets",
                schema: "public",
                table: "workout_activity_assignments",
                type: "integer",
                nullable: true);
        }
    }
}
