using Domain.Notifications;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime DateCreated { get; private set; } = DateTime.UtcNow;

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }


    public UserProfile? Profile { get; set; }
    public UserActivityStreak? ActivityStreak { get; set; }
    public ICollection<UserAchievement> Achievements { get; private set; } = new List<UserAchievement>();
    public ICollection<Notification> Notifications { get; private set; } = new List<Notification>();


    public enum UserRole
    {
        User = 0,
        Admin = 1
    }
    public void SetRefreshToken(string token, DateTime expiryTime)
    {
        RefreshToken = token;
        RefreshTokenExpiryTime = expiryTime;
    }

    public bool IsRefreshTokenValid()
    {
        return RefreshToken != null &&
               RefreshTokenExpiryTime.HasValue &&
               RefreshTokenExpiryTime.Value > DateTime.UtcNow;
    }

    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiryTime = null;
    }
}