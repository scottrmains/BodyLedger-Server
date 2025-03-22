using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class changeexercisetoacitity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "workout_exercise_assignments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workout_exercises",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "fitness_activities",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recommended_duration = table.Column<int>(type: "integer", nullable: false),
                    intensity_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    average_pace = table.Column<decimal>(type: "numeric", nullable: true),
                    fitness_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_fitness_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_fitness_activities_fitness_templates_fitness_template_id",
                        column: x => x.fitness_template_id,
                        principalSchema: "public",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_activities",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    activity_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recommended_sets = table.Column<int>(type: "integer", nullable: false),
                    rep_ranges = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    workout_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workout_activities", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_activities_workout_templates_workout_template_id",
                        column: x => x.workout_template_id,
                        principalSchema: "public",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "fitness_activity_assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fitness_activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed_duration = table.Column<int>(type: "integer", nullable: true),
                    actual_intensity = table.Column<string>(type: "text", nullable: true),
                    actual_pace = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fitness_activity_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_fitness_activity_assignments_assignment_items_id",
                        column: x => x.id,
                        principalSchema: "public",
                        principalTable: "assignment_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_fitness_activity_assignments_fitness_activities_fitness_act",
                        column: x => x.fitness_activity_id,
                        principalSchema: "public",
                        principalTable: "fitness_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_activity_assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_activity_id = table.Column<Guid>(type: "uuid", nullable: false),
                    completed_sets = table.Column<int>(type: "integer", nullable: true),
                    completed_reps = table.Column<int>(type: "integer", nullable: true),
                    actual_weight = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workout_activity_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_activity_assignments_assignment_items_id",
                        column: x => x.id,
                        principalSchema: "public",
                        principalTable: "assignment_items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_workout_activity_assignments_workout_activities_workout_act",
                        column: x => x.workout_activity_id,
                        principalSchema: "public",
                        principalTable: "workout_activities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_fitness_activities_fitness_template_id",
                schema: "public",
                table: "fitness_activities",
                column: "fitness_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_fitness_activity_assignments_fitness_activity_id",
                schema: "public",
                table: "fitness_activity_assignments",
                column: "fitness_activity_id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_activities_workout_template_id",
                schema: "public",
                table: "workout_activities",
                column: "workout_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_activity_assignments_workout_activity_id",
                schema: "public",
                table: "workout_activity_assignments",
                column: "workout_activity_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "fitness_activity_assignments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workout_activity_assignments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "fitness_activities",
                schema: "public");

            migrationBuilder.DropTable(
                name: "workout_activities",
                schema: "public");

            migrationBuilder.CreateTable(
                name: "workout_exercises",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exercise_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    recommended_sets = table.Column<int>(type: "integer", nullable: false),
                    rep_ranges = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_workout_exercises", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_exercises_workout_templates_workout_template_id",
                        column: x => x.workout_template_id,
                        principalSchema: "public",
                        principalTable: "templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "workout_exercise_assignments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    workout_exercise_id = table.Column<Guid>(type: "uuid", nullable: false),
                    actual_weight = table.Column<int>(type: "integer", nullable: true),
                    completed_reps = table.Column<int>(type: "integer", nullable: true),
                    completed_sets = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_workout_exercise_assignments", x => x.id);
                    table.ForeignKey(
                        name: "fk_workout_exercise_assignments_assignment_items_id",
                        column: x => x.id,
                        principalSchema: "public",
                        principalTable: "assignment_items",
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
                name: "ix_workout_exercise_assignments_workout_exercise_id",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "workout_exercise_id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_exercises_workout_template_id",
                schema: "public",
                table: "workout_exercises",
                column: "workout_template_id");
        }
    }
}
