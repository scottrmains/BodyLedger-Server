﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class modelupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "workout_exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "workout_exercises",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "user_profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "user_profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "templates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "templates",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "template_checklists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "template_checklists",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "template_assignments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "template_assignments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_created",
                schema: "public",
                table: "assignment_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "date_updated",
                schema: "public",
                table: "assignment_items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "workout_exercises");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "workout_exercises");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "users");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "user_profiles");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "templates");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "templates");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "template_checklists");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "template_assignments");

            migrationBuilder.DropColumn(
                name: "date_created",
                schema: "public",
                table: "assignment_items");

            migrationBuilder.DropColumn(
                name: "date_updated",
                schema: "public",
                table: "assignment_items");
        }
    }
}
