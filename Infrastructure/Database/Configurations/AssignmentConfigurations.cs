using Domain.Assignments;
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

            builder.HasOne(ai => ai.Assignment)
                .WithMany(ta => ta.Items)
                .HasForeignKey(ai => ai.AssignmentId)
                .IsRequired();
        }
    }

    public class WorkoutSetConfiguration : IEntityTypeConfiguration<WorkoutSet>
    {
        public void Configure(EntityTypeBuilder<WorkoutSet> builder)
        {
            builder.ToTable("workout_sets");
            builder.HasKey(ws => ws.Id);

            builder.Property(ws => ws.SetNumber)
                .IsRequired();

            builder.Property(ws => ws.Reps)
                .IsRequired();

            builder.Property(ws => ws.Weight);

            builder.Property(ws => ws.CreatedAt)
                .IsRequired();

            builder.HasOne(ws => ws.WorkoutActivityAssignment)
                .WithMany(waa => waa.Sets)
                .HasForeignKey(ws => ws.WorkoutActivityAssignmentId)
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