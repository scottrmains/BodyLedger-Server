using Domain.Checklist;
using Domain.Templates;
using Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Database.Configurations
{

    public class WeeklyChecklistConfiguration : IEntityTypeConfiguration<WeeklyChecklist>
    {
        public void Configure(EntityTypeBuilder<WeeklyChecklist> builder)
        {

    
        }
    }

 

}
