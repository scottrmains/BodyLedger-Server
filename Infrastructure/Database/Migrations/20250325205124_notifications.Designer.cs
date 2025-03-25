﻿// <auto-generated />
using System;
using System.Text.Json;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250325205124_notifications")]
    partial class notifications
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Assignments.Assignment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("ChecklistId")
                        .HasColumnType("uuid")
                        .HasColumnName("checklist_id");

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

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("template_id");

                    b.Property<int>("TimeOfDay")
                        .HasColumnType("integer")
                        .HasColumnName("time_of_day");

                    b.HasKey("Id")
                        .HasName("pk_assignments");

                    b.HasIndex("ChecklistId")
                        .HasDatabaseName("ix_assignments_checklist_id");

                    b.HasIndex("TemplateId")
                        .HasDatabaseName("ix_assignments_template_id");

                    b.ToTable("assignments", "public");
                });

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

                    b.HasKey("Id");

                    b.HasIndex("TemplateAssignmentId")
                        .HasDatabaseName("ix_assignment_items_template_assignment_id");

                    b.ToTable("assignment_items", "public");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Domain.Checklists.Checklist", b =>
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
                        .HasName("pk_checklists");

                    b.ToTable("checklists", "public");
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean")
                        .HasColumnName("is_read");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message");

                    b.Property<JsonDocument>("Metadata")
                        .HasColumnType("jsonb")
                        .HasColumnName("metadata");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_notifications");

                    b.ToTable("notifications", "public");
                });

            modelBuilder.Entity("Domain.Templates.Fitness.FitnessActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("activity_name");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_updated");

                    b.Property<Guid>("FitnessTemplateId")
                        .HasColumnType("uuid")
                        .HasColumnName("fitness_template_id");

                    b.Property<string>("IntensityLevel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("intensity_level");

                    b.Property<int>("RecommendedDuration")
                        .HasColumnType("integer")
                        .HasColumnName("recommended_duration");

                    b.HasKey("Id")
                        .HasName("pk_fitness_activities");

                    b.HasIndex("FitnessTemplateId")
                        .HasDatabaseName("ix_fitness_activities_fitness_template_id");

                    b.ToTable("fitness_activities", "public");
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

            modelBuilder.Entity("Domain.Templates.WorkoutActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ActivityName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("activity_name");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_updated");

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
                        .HasName("pk_workout_activities");

                    b.HasIndex("WorkoutTemplateId")
                        .HasDatabaseName("ix_workout_activities_workout_template_id");

                    b.ToTable("workout_activities", "public");
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

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expiry_time");

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

            modelBuilder.Entity("Domain.Assignments.FitnessActivityAssignment", b =>
                {
                    b.HasBaseType("Domain.Assignments.AssignmentItem");

                    b.Property<string>("ActualIntensity")
                        .HasColumnType("text")
                        .HasColumnName("actual_intensity");

                    b.Property<decimal?>("ActualPace")
                        .HasColumnType("numeric")
                        .HasColumnName("actual_pace");

                    b.Property<int?>("CompletedDuration")
                        .HasColumnType("integer")
                        .HasColumnName("completed_duration");

                    b.Property<Guid>("FitnessActivityId")
                        .HasColumnType("uuid")
                        .HasColumnName("fitness_activity_id");

                    b.HasIndex("FitnessActivityId")
                        .HasDatabaseName("ix_fitness_activity_assignments_fitness_activity_id");

                    b.ToTable("fitness_activity_assignments", "public");
                });

            modelBuilder.Entity("Domain.TemplateAssignments.WorkoutActivityAssignment", b =>
                {
                    b.HasBaseType("Domain.Assignments.AssignmentItem");

                    b.Property<int?>("ActualWeight")
                        .HasColumnType("integer")
                        .HasColumnName("actual_weight");

                    b.Property<int?>("CompletedReps")
                        .HasColumnType("integer")
                        .HasColumnName("completed_reps");

                    b.Property<int?>("CompletedSets")
                        .HasColumnType("integer")
                        .HasColumnName("completed_sets");

                    b.Property<Guid>("WorkoutActivityId")
                        .HasColumnType("uuid")
                        .HasColumnName("workout_activity_id");

                    b.HasIndex("WorkoutActivityId")
                        .HasDatabaseName("ix_workout_activity_assignments_workout_activity_id");

                    b.ToTable("workout_activity_assignments", "public");
                });

            modelBuilder.Entity("Domain.Templates.Fitness.FitnessTemplate", b =>
                {
                    b.HasBaseType("Domain.Templates.Template");

                    b.HasDiscriminator().HasValue("fitness");
                });

            modelBuilder.Entity("Domain.Templates.WorkoutTemplate", b =>
                {
                    b.HasBaseType("Domain.Templates.Template");

                    b.HasDiscriminator().HasValue("workout");
                });

            modelBuilder.Entity("Domain.Assignments.Assignment", b =>
                {
                    b.HasOne("Domain.Checklists.Checklist", "Checklist")
                        .WithMany("Assignments")
                        .HasForeignKey("ChecklistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_assignments_checklists_checklist_id");

                    b.HasOne("Domain.Templates.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_assignments_templates_template_id");

                    b.Navigation("Checklist");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Domain.Assignments.AssignmentItem", b =>
                {
                    b.HasOne("Domain.Assignments.Assignment", "TemplateAssignment")
                        .WithMany("Items")
                        .HasForeignKey("TemplateAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_assignment_items_assignments_template_assignment_id");

                    b.Navigation("TemplateAssignment");
                });

            modelBuilder.Entity("Domain.Templates.Fitness.FitnessActivity", b =>
                {
                    b.HasOne("Domain.Templates.Fitness.FitnessTemplate", "FitnessTemplate")
                        .WithMany("Activities")
                        .HasForeignKey("FitnessTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_fitness_activities_fitness_templates_fitness_template_id");

                    b.Navigation("FitnessTemplate");
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

            modelBuilder.Entity("Domain.Templates.WorkoutActivity", b =>
                {
                    b.HasOne("Domain.Templates.WorkoutTemplate", "WorkoutTemplate")
                        .WithMany("Activities")
                        .HasForeignKey("WorkoutTemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_activities_workout_templates_workout_template_id");

                    b.Navigation("WorkoutTemplate");
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

            modelBuilder.Entity("Domain.Assignments.FitnessActivityAssignment", b =>
                {
                    b.HasOne("Domain.Templates.Fitness.FitnessActivity", "FitnessExercise")
                        .WithMany()
                        .HasForeignKey("FitnessActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_fitness_activity_assignments_fitness_activities_fitness_act");

                    b.HasOne("Domain.Assignments.AssignmentItem", null)
                        .WithOne()
                        .HasForeignKey("Domain.Assignments.FitnessActivityAssignment", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_fitness_activity_assignments_assignment_items_id");

                    b.Navigation("FitnessExercise");
                });

            modelBuilder.Entity("Domain.TemplateAssignments.WorkoutActivityAssignment", b =>
                {
                    b.HasOne("Domain.Assignments.AssignmentItem", null)
                        .WithOne()
                        .HasForeignKey("Domain.TemplateAssignments.WorkoutActivityAssignment", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_activity_assignments_assignment_items_id");

                    b.HasOne("Domain.Templates.WorkoutActivity", "WorkoutActivity")
                        .WithMany()
                        .HasForeignKey("WorkoutActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_activity_assignments_workout_activities_workout_act");

                    b.Navigation("WorkoutActivity");
                });

            modelBuilder.Entity("Domain.Assignments.Assignment", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.Checklists.Checklist", b =>
                {
                    b.Navigation("Assignments");
                });

            modelBuilder.Entity("Domain.Templates.Fitness.FitnessTemplate", b =>
                {
                    b.Navigation("Activities");
                });

            modelBuilder.Entity("Domain.Templates.WorkoutTemplate", b =>
                {
                    b.Navigation("Activities");
                });
#pragma warning restore 612, 618
        }
    }
}
