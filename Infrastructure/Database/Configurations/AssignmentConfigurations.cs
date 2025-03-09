using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Assignments;
using Domain.TemplateAssignments;

namespace Infrastructure.Database.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("assignments");

            builder.HasKey(ta => ta.Id);
            builder.Property(ta => ta.ScheduledDay)
                .IsRequired();

            // Configure relationship with the abstract Template.
            // Since Template is the base class for all templates (e.g. WorkoutTemplate),
            // EF Core will handle this relationship if using TPH.
            builder.HasOne(ta => ta.Template)
                .WithMany()  // You could also add a collection property on Template if desired.
                .HasForeignKey(ta => ta.TemplateId)
                .IsRequired();
        }
    }

    // Configuration for AssignmentItem (if you use sub-items, e.g. individual exercise completions)
    public class AssignmentItemConfiguration : IEntityTypeConfiguration<AssignmentItem>
    {
        public void Configure(EntityTypeBuilder<AssignmentItem> builder)
        {
            builder.ToTable("assignment_items");
            builder.HasKey(ai => ai.Id);

            // Add this line to make it use TPT inheritance
            builder.UseTptMappingStrategy();

            // Configure additional properties for AssignmentItem as needed.
            // Establish relationship with TemplateAssignment
            builder.HasOne(ai => ai.TemplateAssignment)
                .WithMany(ta => ta.Items)
                .HasForeignKey(ai => ai.TemplateAssignmentId)
                .IsRequired();
        }
    }

    public class WorkoutExerciseAssignmentConfiguration : IEntityTypeConfiguration<WorkoutExerciseAssignment>
    {
        public void Configure(EntityTypeBuilder<WorkoutExerciseAssignment> builder)
        {
            // Specify that this is a derived type with its own table
            builder.ToTable("workout_exercise_assignments");

            // Configure specific properties
            builder.Property(wea => wea.CompletedSets);
            builder.Property(wea => wea.CompletedReps);
            builder.Property(wea => wea.ActualWeight);

            // Configure relationship with WorkoutExercise
            builder.HasOne(wea => wea.WorkoutExercise)
                .WithMany()
                .HasForeignKey(wea => wea.WorkoutExerciseId)
                .IsRequired();
        }
    }
}
