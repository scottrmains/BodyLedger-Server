
using Domain.Checklists;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{

    public class WeeklyChecklistConfiguration : IEntityTypeConfiguration<Checklist>
    {
        public void Configure(EntityTypeBuilder<Checklist> builder)
        {

    
        }
    }

 

}
