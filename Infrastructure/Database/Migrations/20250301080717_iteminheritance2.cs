using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class iteminheritance2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workout_exercise_assignments_template_assignments_template_",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_workout_exercise_assignments",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropIndex(
                name: "ix_workout_exercise_assignments_template_assignment_id",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_assignment_items",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.DropColumn(
                name: "completed_date",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropColumn(
                name: "template_assignment_id",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_workout_exercise_assignments",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_assignment_items",
                schema: "public",
                table: "assignment_items",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_workout_exercise_assignments_assignment_items_id",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "id",
                principalSchema: "public",
                principalTable: "assignment_items",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_workout_exercise_assignments_assignment_items_id",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_workout_exercise_assignments",
                schema: "public",
                table: "workout_exercise_assignments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_assignment_items",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.AddColumn<DateTime>(
                name: "completed_date",
                schema: "public",
                table: "workout_exercise_assignments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "template_assignment_id",
                schema: "public",
                table: "workout_exercise_assignments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_workout_exercise_assignments",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_assignment_items",
                schema: "public",
                table: "assignment_items",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_workout_exercise_assignments_template_assignment_id",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "template_assignment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_workout_exercise_assignments_template_assignments_template_",
                schema: "public",
                table: "workout_exercise_assignments",
                column: "template_assignment_id",
                principalSchema: "public",
                principalTable: "template_assignments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
