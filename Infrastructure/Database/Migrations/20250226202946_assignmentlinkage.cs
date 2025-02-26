using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class assignmentlinkage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "weight",
                schema: "public",
                table: "workout_exercises");

            migrationBuilder.AddColumn<DateTime>(
                name: "completed_date",
                schema: "public",
                table: "assignment_items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "workout_exercise_assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    template_assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed_sets = table.Column<int>(type: "integer", nullable: true),
                    completed_reps = table.Column<int>(type: "integer", nullable: true),
                    actual_weight = table.Column<int>(type: "integer", nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    completed_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workout_exercise_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_exercise_assignments_template_assignments_template_",
                        column: x => x.template_assignment_id,
                        principalSchema: "public",
                        principalTable: "template_assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_workout_exercise_assignments_workout_exercises_workout_exer",
                        column: x => x.workout_exercise_id,
                        principalSchema: "public",
                        principalTable: "workout_exercises",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_workout_exercise_assignments_template_assignment_id",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "template_assignment_id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_exercise_assignments_workout_exercise_id",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "workout_exercise_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workout_exercise_assignments",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "completed_date",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.AddColumn<int>(
                name: "weight",
                schema: "public",
                table: "workout_exercises",
                type: "integer",
                nullable: true);
        }
    }
}
