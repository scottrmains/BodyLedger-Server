﻿// <auto-generated />
using System;
using System.Text.Json;
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

                    b.Property<Guid>("AssignmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("assignment_id");

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<DateTime?>("CompletedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completed_date");

                    b.HasKey("Id");

                    b.HasIndex("AssignmentId")
                        .HasDatabaseName("ix_assignment_items_assignment_id");

                    b.ToTable("assignment_items", "public");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Domain.Assignments.WorkoutSet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int>("Reps")
                        .HasColumnType("integer")
                        .HasColumnName("reps");

                    b.Property<int>("SetNumber")
                        .HasColumnType("integer")
                        .HasColumnName("set_number");

                    b.Property<int?>("Weight")
                        .HasColumnType("integer")
                        .HasColumnName("weight");

                    b.Property<Guid>("WorkoutActivityAssignmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("workout_activity_assignment_id");

                    b.HasKey("Id")
                        .HasName("pk_workout_sets");

                    b.HasIndex("WorkoutActivityAssignmentId")
                        .HasDatabaseName("ix_workout_sets_workout_activity_assignment_id");

                    b.ToTable("workout_sets", "public");
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
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("message");

                    b.Property<JsonDocument>("Metadata")
                        .HasColumnType("jsonb")
                        .HasColumnName("metadata");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("title");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_notifications");

                    b.HasIndex("UserId", "IsRead")
                        .HasDatabaseName("ix_notifications_user_id_is_read");

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

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("refresh_token");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("refresh_token_expiry_time");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", "public");
                });

            modelBuilder.Entity("Domain.Users.UserAchievement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("description");

                    b.Property<DateTime>("EarnedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("earned_at");

                    b.Property<bool>("IsNotified")
                        .HasColumnType("boolean")
                        .HasColumnName("is_notified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_achievements");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_user_achievements_user_id");

                    b.ToTable("user_achievements", "public");
                });

            modelBuilder.Entity("Domain.Users.UserActivityStreak", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("CompletedActivitiesCount")
                        .HasColumnType("integer")
                        .HasColumnName("completed_activities_count");

                    b.Property<int>("CurrentStreak")
                        .HasColumnType("integer")
                        .HasColumnName("current_streak");

                    b.Property<DateTime>("LastActivityDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_activity_date");

                    b.Property<int>("LongestStreak")
                        .HasColumnType("integer")
                        .HasColumnName("longest_streak");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_user_activity_streaks");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("ix_user_activity_streaks_user_id");

                    b.ToTable("user_activity_streaks", "public");
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
                        .IsUnique()
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

            modelBuilder.Entity("Domain.Assignments.WorkoutActivityAssignment", b =>
                {
                    b.HasBaseType("Domain.Assignments.AssignmentItem");

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
                    b.HasOne("Domain.Assignments.Assignment", "Assignment")
                        .WithMany("Items")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_assignment_items_assignments_assignment_id");

                    b.Navigation("Assignment");
                });

            modelBuilder.Entity("Domain.Assignments.WorkoutSet", b =>
                {
                    b.HasOne("Domain.Assignments.WorkoutActivityAssignment", "WorkoutActivityAssignment")
                        .WithMany("Sets")
                        .HasForeignKey("WorkoutActivityAssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_workout_sets_workout_activity_assignments_workout_activity_");

                    b.Navigation("WorkoutActivityAssignment");
                });

            modelBuilder.Entity("Domain.Notifications.Notification", b =>
                {
                    b.HasOne("Domain.Users.User", null)
                        .WithMany("Notifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_notifications_users_user_id");
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

            modelBuilder.Entity("Domain.Users.UserAchievement", b =>
                {
                    b.HasOne("Domain.Users.User", null)
                        .WithMany("Achievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_achievements_users_user_id");
                });

            modelBuilder.Entity("Domain.Users.UserActivityStreak", b =>
                {
                    b.HasOne("Domain.Users.User", null)
                        .WithOne("ActivityStreak")
                        .HasForeignKey("Domain.Users.UserActivityStreak", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_user_activity_streaks_users_user_id");
                });

            modelBuilder.Entity("Domain.Users.UserProfile", b =>
                {
                    b.HasOne("Domain.Users.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("Domain.Users.UserProfile", "UserId")
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

            modelBuilder.Entity("Domain.Assignments.WorkoutActivityAssignment", b =>
                {
                    b.HasOne("Domain.Assignments.AssignmentItem", null)
                        .WithOne()
                        .HasForeignKey("Domain.Assignments.WorkoutActivityAssignment", "Id")
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

            modelBuilder.Entity("Domain.Users.User", b =>
                {
                    b.Navigation("Achievements");

                    b.Navigation("ActivityStreak");

                    b.Navigation("Notifications");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Domain.Assignments.WorkoutActivityAssignment", b =>
                {
                    b.Navigation("Sets");
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
