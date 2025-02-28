﻿// <auto-generated />
using System;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Assignments.AssignmentItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_date");

                    b.Property<Guid>("TemplateAssignmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_assignment_id");

                    b.HasKey("Id")
                        .HasName("pk_assignment_items");

                    b.HasIndex("TemplateAssignmentId")
                        .HasDatabaseName("ix_assignment_items_template_assignment_id");

                    b.ToTable("assignment_items", "public");
                });

            modelBuilder.Entity("Domain.Assignments.TemplateAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_date");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("boolean")
                        .HasColumnName("is_recurring");

                    b.Property<DateTime>("RecurringStartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("recurring_start_date");

                    b.Property<int>("ScheduledDay")
                        .HasColumnType("integer")
                        .HasColumnName("scheduled_day");

                    b.Property<Guid>("TemplateChecklistId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_checklist_id");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_id");

                    b.Property<int>("TimeOfDay")
                        .HasColumnType("integer")
                        .HasColumnName("time_of_day");

                    b.HasKey("Id")
                        .HasName("pk_template_assignments");

                    b.HasIndex("TemplateChecklistId")
                        .HasDatabaseName("ix_template_assignments_template_checklist_id");

                    b.HasIndex("TemplateId")
                        .HasDatabaseName("ix_template_assignments_template_id");

                    b.ToTable("template_assignments", "public");
                });

            modelBuilder.Entity("Domain.Checklist.WeeklyChecklist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_updated");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_date");

                    b.Property<int>("StartDay")
                        .HasColumnType("integer")
                        .HasColumnName("start_day");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_weekly_checklists");

                    b.ToTable("weekly_checklists", "public");
                });

            modelBuilder.Entity("Domain.TemplateAssignments.WorkoutExerciseAssignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int?>("ActualWeight")
                        .HasColumnType("integer")
                        .HasColumnName("actual_weight");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_date");

                    b.Property<int?>("CompletedReps")
                        .HasColumnType("integer")
                        .HasColumnName("completed_reps");

                    b.Property<int?>("CompletedSets")
                        .HasColumnType("integer")
                        .HasColumnName("completed_sets");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_completed");

                    b.Property<Guid>("TemplateAssignmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_assignment_id");

                    b.Property<Guid>("WorkoutExerciseId")
                        .HasColumnType("uuid")
                        .HasColumnName("workout_exercise_id");

                    b.HasKey("Id")
                        .HasName("pk_workout_exercise_assignments");

                    b.HasIndex("TemplateAssignmentId")
                        .HasDatabaseName("ix_workout_exercise_assignments_template_assignment_id");

                    b.HasIndex("WorkoutExerciseId")
                        .HasDatabaseName("ix_workout_exercise_assignments_workout_exercise_id");

                    b.ToTable("workout_exercise_assignments", "public");
                });

            modelBuilder.Entity("Domain.Templates.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_updated");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("name");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("template_type")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)")
                        .HasColumnName("template_type");

                    b.HasKey("Id")
                        .HasName("pk_templates");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_templates_user_id");

                    b.ToTable("templates", "public");

                    b.HasDiscriminator<string>("template_type").HasValue("Template");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("last_name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "public");
                });

            modelBuilder.Entity("Domain.Users.UserProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<TimeSpan?>("CurrentPace")
                        .HasColumnType("interval")
                        .HasColumnName("current_pace");

                    b.Property<double?>("CurrentWeight")
                        .HasColumnType("double precision")
                        .HasColumnName("current_weight");

                    b.Property<TimeSpan?>("GoalPace")
                        .HasColumnType("interval")
                        .HasColumnName("goal_pace");

                    b.Property<double?>("GoalWeight")
                        .HasColumnType("double precision")
                        .HasColumnName("goal_weight");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_profiles");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_profiles_user_id");

                    b.ToTable("user_profiles", "public");
                });

            modelBuilder.Entity("Domain.Workouts.WorkoutExercise", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_updated");

                    b.Property<string>("ExerciseName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("exercise_name");

                    b.Property<int>("RecommendedSets")
                        .HasColumnType("integer")
                        .HasColumnName("recommended_sets");

                    b.Property<string>("RepRanges")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("rep_ranges");

                    b.Property<Guid>("WorkoutTemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("workout_template_id");

                    b.HasKey("Id")
                        .HasName("pk_workout_exercises");

                    b.HasIndex("WorkoutTemplateId")
                        .HasDatabaseName("ix_workout_exercises_workout_template_id");

                    b.ToTable("workout_exercises", "public");
                });

            modelBuilder.Entity("Domain.Workouts.WorkoutTemplate", b =>
                {
                    b.HasBaseType("Domain.Templates.Template");

                    b.HasDiscriminator().HasValue("workout");
                });

            modelBuilder.Entity("Domain.Assignments.AssignmentItem", b =>
                {
                    b.HasOne("Domain.Assignments.TemplateAssignment", null)
                        .WithMany("Items")
                        .HasForeignKey("TemplateAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_assignment_items_template_assignments_template_assignment_id");
                });

            modelBuilder.Entity("Domain.Assignments.TemplateAssignment", b =>
                {
                    b.HasOne("Domain.Checklist.WeeklyChecklist", "TemplateChecklist")
                        .WithMany("Assignments")
                        .HasForeignKey("TemplateChecklistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_template_assignments_weekly_checklists_template_checklist_id");

                    b.HasOne("Domain.Templates.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_template_assignments_templates_template_id");

                    b.Navigation("Template");

                    b.Navigation("TemplateChecklist");
                });

            modelBuilder.Entity("Domain.TemplateAssignments.WorkoutExerciseAssignment", b =>
                {
                    b.HasOne("Domain.Assignments.TemplateAssignment", "TemplateAssignment")
                        .WithMany()
                        .HasForeignKey("TemplateAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_exercise_assignments_template_assignments_template_");

                    b.HasOne("Domain.Workouts.WorkoutExercise", "WorkoutExercise")
                        .WithMany()
                        .HasForeignKey("WorkoutExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_exercise_assignments_workout_exercises_workout_exer");

                    b.Navigation("TemplateAssignment");

                    b.Navigation("WorkoutExercise");
                });

            modelBuilder.Entity("Domain.Templates.Template", b =>
                {
                    b.HasOne("Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_templates_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Users.UserProfile", b =>
                {
                    b.HasOne("Domain.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_profiles_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Workouts.WorkoutExercise", b =>
                {
                    b.HasOne("Domain.Workouts.WorkoutTemplate", "WorkoutTemplate")
                        .WithMany("Exercises")
                        .HasForeignKey("WorkoutTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_exercises_workout_templates_workout_template_id");

                    b.Navigation("WorkoutTemplate");
                });

            modelBuilder.Entity("Domain.Assignments.TemplateAssignment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Checklist.WeeklyChecklist", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Domain.Workouts.WorkoutTemplate", b =>
                {
                    b.Navigation("Exercises");
                });
#pragma warning restore 612, 618
        }
    }
}
