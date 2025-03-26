using SharedKernel;
using SharedKernel.Enums;

namespace Domain.Users;



public class UserAchievement : Entity
{
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public AchievementType Type { get; private set; }
    public DateTime EarnedAt { get; private set; }
    public bool IsNotified { get; private set; }

    private UserAchievement() { }

    public UserAchievement(
        Guid userId,
        string name,
        string description,
        AchievementType type)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Type = type;
        EarnedAt = DateTime.UtcNow;
        IsNotified = false;

        Raise(new AchievementEarnedDomainEvent(Id, UserId, Name));
    }

    public void MarkNotified()
    {
        IsNotified = true;
    }

    public static UserAchievement CreateFirstCompletionAchievement(Guid userId)
    {
        return new UserAchievement(
            userId,
            "First Step",
            "You completed your first activity. Great start to your journey!",
            AchievementType.FirstCompletion);
    }

    public static UserAchievement CreateStreakAchievement(Guid userId, int days)
    {
        return new UserAchievement(
            userId,
            $"{days}-Day Streak",
            $"You've completed activities for {days} consecutive days!",
            AchievementType.Streak);
    }

    public static UserAchievement CreateCompletionCountAchievement(Guid userId, int count)
    {
        return new UserAchievement(
            userId,
            $"{count} Activities Completed",
            $"You've completed {count} activities. Your dedication is inspiring!",
            AchievementType.CompletionCount);
    }

    public static UserAchievement CreatePerfectWeekAchievement(Guid userId)
    {
        return new UserAchievement(
            userId,
            "Perfect Week",
            "You've completed all scheduled activities for the week. Amazing discipline!",
            AchievementType.PerfectWeek);
    }
}

public sealed record AchievementEarnedDomainEvent(Guid AchievementId, Guid UserId, string AchievementName) : IDomainEvent;