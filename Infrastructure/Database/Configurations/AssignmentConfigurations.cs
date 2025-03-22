using Domain.Assignments;
using Domain.TemplateAssignments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder.HasOne(ta => ta.Template)
                .WithMany()
                .HasForeignKey(ta => ta.TemplateId)
                .IsRequired();
        }
    }

    public class AssignmentItemConfiguration : IEntityTypeConfiguration<AssignmentItem>
    {
        public void Configure(EntityTypeBuilder<AssignmentItem> builder)
        {
            builder.ToTable("assignment_items");
            builder.HasKey(ai => ai.Id);
            builder.UseTptMappingStrategy();

            builder.HasOne(ai => ai.TemplateAssignment)
                .WithMany(ta => ta.Items)
                .HasForeignKey(ai => ai.TemplateAssignmentId)
                .IsRequired();
        }
    }

    public class WorkoutExerciseAssignmentConfiguration : IEntityTypeConfiguration<WorkoutActivityAssignment>
    {
        public void Configure(EntityTypeBuilder<WorkoutActivityAssignment> builder)
        {
            builder.ToTable("workout_activity_assignments");
            builder.Property(wea => wea.CompletedSets);
            builder.Property(wea => wea.CompletedReps);
            builder.Property(wea => wea.ActualWeight);

            builder.HasOne(wea => wea.WorkoutActivity)
                .WithMany()
                .HasForeignKey(wea => wea.WorkoutActivityId)
                .IsRequired();
        }
    }

    public class FitnessExerciseAssignmentConfiguration : IEntityTypeConfiguration<FitnessActivityAssignment>
    {
        public void Configure(EntityTypeBuilder<FitnessActivityAssignment> builder)
        {
            builder.ToTable("fitness_activity_assignments");
            builder.Property(fea => fea.CompletedDuration);
            builder.Property(fea => fea.ActualIntensity);

            builder.HasOne(fea => fea.FitnessExercise)
                .WithMany()
                .HasForeignKey(fea => fea.FitnessActivityId)
                .IsRequired();
        }
    }
}