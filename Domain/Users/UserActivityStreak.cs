using SharedKernel;

namespace Domain.Users;

public class UserActivityStreak : Entity
{
    public Guid UserId { get; private set; }
    public int CurrentStreak { get; private set; }
    public int LongestStreak { get; private set; }
    public DateTime LastActivityDate { get; private set; }
    public int CompletedActivitiesCount { get; private set; }

    private UserActivityStreak() { }

    public UserActivityStreak(Guid userId)
    {
        UserId = userId;
        CurrentStreak = 0;
        LongestStreak = 0;
        LastActivityDate = DateTime.MinValue;
        CompletedActivitiesCount = 0;
    }

    public void RecordActivity(DateTime activityDate)
    {
        activityDate = activityDate.Date;

        // Increment total count
        CompletedActivitiesCount++;

        // If this is the first activity ever or it's a new day
        if (LastActivityDate == DateTime.MinValue)
        {
            CurrentStreak = 1;
            LastActivityDate = activityDate;
            return;
        }

        // Check if this is the same day (already counted)
        if (activityDate.Date == LastActivityDate.Date)
        {
            return;
        }

        // Check if this is the very next day
        if (activityDate.Date == LastActivityDate.Date.AddDays(1))
        {
            CurrentStreak++;

            // Update longest streak if current is greater
            if (CurrentStreak > LongestStreak)
            {
                LongestStreak = CurrentStreak;

                // Raise domain events for milestone streaks
                if (IsStreakMilestone(CurrentStreak))
                {
                    Raise(new StreakMilestoneDomainEvent(UserId, CurrentStreak));
                }
            }
        }
        // If it's a gap larger than 1 day, restart streak
        else if (activityDate > LastActivityDate.AddDays(1))
        {
            CurrentStreak = 1;
        }

        LastActivityDate = activityDate;

        // Check for completion count milestones
        if (IsCompletionCountMilestone(CompletedActivitiesCount))
        {
            Raise(new CompletionCountMilestoneDomainEvent(UserId, CompletedActivitiesCount));
        }
    }

    private bool IsStreakMilestone(int streak)
    {
        // Milestone streaks: 3, 5, 7, 14, 21, 30, 60, 90, etc.
        return streak == 3 || streak == 5 || streak == 7 || streak == 14 ||
               streak == 21 || streak == 30 || streak == 60 || streak == 90 ||
               streak == 100 || streak == 200 || streak == 365;
    }

    private bool IsCompletionCountMilestone(int count)
    {
        // Milestone counts: 1, 5, 10, 25, 50, 100, 250, 500, 1000, etc.
        return count == 1 || count == 5 || count == 10 || count == 25 ||
               count == 50 || count == 100 || count == 250 || count == 500 ||
               count == 1000;
    }
}

public sealed record StreakMilestoneDomainEvent(Guid UserId, int StreakDays) : IDomainEvent;
public sealed record CompletionCountMilestoneDomainEvent(Guid UserId, int CompletionCount) : IDomainEvent;