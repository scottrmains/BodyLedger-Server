using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Templates.Fitness;

namespace Infrastructure.Database.Configurations
{
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.ToTable("templates");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasDiscriminator<string>("template_type")
                .HasValue<WorkoutTemplate>("workout")
                .HasValue<FitnessTemplate>("fitness");
        }
    }

    public class WorkoutTemplateConfiguration : IEntityTypeConfiguration<WorkoutTemplate>
    {
        public void Configure(EntityTypeBuilder<WorkoutTemplate> builder)
        {
            builder.HasBaseType<Template>();

            builder.HasMany(wt => wt.Activities)
                .WithOne(e => e.WorkoutTemplate)
                .HasForeignKey(e => e.WorkoutTemplateId)
                .IsRequired();
        }
    }

    public class FitnessTemplateConfiguration : IEntityTypeConfiguration<FitnessTemplate>
    {
        public void Configure(EntityTypeBuilder<FitnessTemplate> builder)
        {
            builder.HasBaseType<Template>();

            builder.HasMany(ft => ft.Activities)
                .WithOne(e => e.FitnessTemplate)
                .HasForeignKey(e => e.FitnessTemplateId)
                .IsRequired();
        }
    }

    public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutActivity>
    {
        public void Configure(EntityTypeBuilder<WorkoutActivity> builder)
        {
            builder.ToTable("workout_activities");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ActivityName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.RecommendedSets)
                .IsRequired();
            builder.Property(e => e.RepRanges)
                .IsRequired()
                .HasMaxLength(50);
        }
    }

    public class FitnessExerciseConfiguration : IEntityTypeConfiguration<FitnessActivity>
    {
        public void Configure(EntityTypeBuilder<FitnessActivity> builder)
        {
            builder.ToTable("fitness_activities");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ActivityName)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.RecommendedDuration)
                .IsRequired();
            builder.Property(e => e.IntensityLevel)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}