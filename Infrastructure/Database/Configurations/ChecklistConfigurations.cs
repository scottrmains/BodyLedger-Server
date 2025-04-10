
using Domain.Checklists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{

    public class ChecklistConfiguration : IEntityTypeConfiguration<Checklist>
    {
        public void Configure(EntityTypeBuilder<Checklist> builder)
        {
            builder.ToTable("checklists");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.StartDay)
                .IsRequired();

            builder.Property(c => c.StartDate)
                .IsRequired();

            builder.Property(c => c.DateCreated)
                .IsRequired();

            builder.Property(c => c.DateUpdated)
                .IsRequired();

            builder.HasMany(c => c.Assignments)
                .WithOne(a => a.Checklist)
                .HasForeignKey(a => a.ChecklistId)
                .IsRequired();

            builder.HasOne(c => c.Log)
                .WithOne(l => l.Checklist)
                .HasForeignKey<ChecklistLog>(l => l.ChecklistId)
                .IsRequired();

        }

        public void Configure(EntityTypeBuilder<ChecklistLog> builder)
        {
            builder.ToTable("checklist_logs");
            builder.HasKey(cl => cl.Id);

            builder.Property(cl => cl.Date)
                .IsRequired();

            builder.Property(cl => cl.Weight);

            builder.Property(cl => cl.Notes)
                .HasMaxLength(1000);

            builder.Property(cl => cl.Mood)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(cl => cl.CreatedAt)
                .IsRequired();

            builder.Property(cl => cl.UpdatedAt)
                .IsRequired();

            builder.HasOne(cl => cl.Checklist)
                .WithMany()
                .HasForeignKey(cl => cl.ChecklistId)
                .IsRequired();
        }
    }

 

}
