
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Domain.Users;

namespace Infrastructure.Database.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {

            builder.HasKey(t => t.Id);
            builder.HasOne(u => u.User);
 
        }
    }
}
