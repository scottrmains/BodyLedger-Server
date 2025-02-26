using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Assignments;

namespace Infrastructure.Database.Configurations
{
    public class TemplateAssignmentConfiguration : IEntityTypeConfiguration<TemplateAssignment>
    {
        public void Configure(EntityTypeBuilder<TemplateAssignment> builder)
        {
            builder.ToTable("template_assignments");

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
            // Configure additional properties for AssignmentItem as needed.

            // Establish relationship with TemplateAssignment
            // Assuming each AssignmentItem belongs to one TemplateAssignment.
            builder.HasOne<TemplateAssignment>()
                .WithMany(ta => ta.Items)
                .HasForeignKey("TemplateAssignmentId")
                .IsRequired();
        }
    }
}
