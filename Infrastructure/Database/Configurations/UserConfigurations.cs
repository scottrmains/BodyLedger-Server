
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Domain.Users;
using Domain.Notifications;

namespace Infrastructure.Database.Configurations
{

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.RefreshToken)
                .HasMaxLength(500);

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(u => u.DateCreated)
                .IsRequired();

            // Define relationships
            builder.HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            builder.HasOne(u => u.ActivityStreak)
                .WithOne()
                .HasForeignKey<UserActivityStreak>(s => s.UserId);

            builder.HasMany(u => u.Achievements)
                .WithOne()
                .HasForeignKey(a => a.UserId);

            builder.HasMany(u => u.Notifications)
                .WithOne()
                .HasForeignKey(n => n.UserId);
        }
    }
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {

            builder.HasKey(t => t.Id);
            builder.HasOne(u => u.User);

        }
    }

    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");

            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(n => n.IsRead)
                .IsRequired();

            builder.Property(n => n.CreatedAt)
                .IsRequired();

            builder.Property(n => n.Metadata)
                .HasColumnType("jsonb");

            builder.HasIndex(n => new { n.UserId, n.IsRead });
        }
    }


    public class UserAchievementConfiguration : IEntityTypeConfiguration<UserAchievement>
    {
        public void Configure(EntityTypeBuilder<UserAchievement> builder)
        {
            builder.ToTable("user_achievements");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.EarnedAt)
                .IsRequired();

            builder.Property(a => a.IsNotified)
                .IsRequired();
            builder.HasIndex(a => a.UserId);
        }
    }

    public class UserActivityStreakConfiguration : IEntityTypeConfiguration<UserActivityStreak>
    {
        public void Configure(EntityTypeBuilder<UserActivityStreak> builder)
        {
            builder.ToTable("user_activity_streaks");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.CurrentStreak)
                .IsRequired();

            builder.Property(s => s.LongestStreak)
                .IsRequired();

            builder.Property(s => s.LastActivityDate)
                .IsRequired();

            builder.Property(s => s.CompletedActivitiesCount)
                .IsRequired();

            builder.HasIndex(s => s.UserId)
                .IsUnique();
        }
    }
}

