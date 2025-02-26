using Domain.Templates;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Workouts;

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
                .HasValue<WorkoutTemplate>("workout");

        }

        public class WorkoutTemplateConfiguration : IEntityTypeConfiguration<WorkoutTemplate>
        {
            public void Configure(EntityTypeBuilder<WorkoutTemplate> builder)
            {

                builder.HasBaseType<Template>();

                builder.HasMany(wt => wt.Exercises)
                    .WithOne(e => e.WorkoutTemplate)
                    .HasForeignKey(e => e.WorkoutTemplateId)
                    .IsRequired();
            }
        }

        public class WorkoutExerciseConfiguration : IEntityTypeConfiguration<WorkoutExercise>
        {
            public void Configure(EntityTypeBuilder<WorkoutExercise> builder)
            {
                builder.ToTable("workout_exercises");

                builder.HasKey(e => e.Id);

                builder.Property(e => e.ExerciseName)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(e => e.RecommendedSets)
                    .IsRequired();

                builder.Property(e => e.RepRanges)
                    .IsRequired()
                    .HasMaxLength(50);
            }
        }
    }
}
