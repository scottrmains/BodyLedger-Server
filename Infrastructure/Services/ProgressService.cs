using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Application.Progress.GetMonthlyProgress;
using Domain.Assignments;
using Domain.Checklists;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Enums;
using SharedKernel.Responses;
using System.Globalization;

namespace Infrastructure.Services;

public class ProgressService(IApplicationDbContext context) : IProgressService
{
    public async Task<MonthlyProgressResponse> GetMonthlyProgressAsync(
        Guid userId,
        int year,
        int month,
        CancellationToken cancellationToken)
    {
        // Calculate the date range for the specified month
        var startDate = new DateTime(year, month, 1).ToUniversalTime();
        var endDate = startDate.AddMonths(1).AddDays(-1).ToUniversalTime();

        // Create the response object
        var response = new MonthlyProgressResponse
        {
            Year = year,
            Month = month,
            MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)
        };

        // Get all checklists for the month
        var checklists = await GetChecklistsForMonth(userId, startDate, endDate, cancellationToken);

        // Process assignment data
        ProcessAssignmentData(response, checklists);

        // Process weight and mood logs
        ProcessWeightAndMoodLogs(response, checklists);

        // Get achievements for the month
        response.Achievements = await GetAchievementsForMonth(userId, startDate, endDate, cancellationToken);

        // Build weekly progress data
        response.WeeklyProgress = BuildWeeklyProgress(checklists, startDate, endDate);

        // Fetch workout metrics (sets, reps, weight)
        response.WorkoutMetrics = await GetWorkoutMetrics(userId, startDate, endDate, cancellationToken);

        // Fetch fitness metrics (duration, intensity)
        response.FitnessMetrics = await GetFitnessMetrics(userId, startDate, endDate, cancellationToken);

        // Get user streak information
        response.StreakInfo = await GetUserStreakInfo(userId, cancellationToken);

        return response;
    }

    private async Task<List<Checklist>> GetChecklistsForMonth(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {

        return await context.Checklists
            .Where(c => c.UserId == userId &&
                   c.StartDate <= endDate &&
                   c.StartDate.AddDays(6) >= startDate)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.Template)
            .Include(c => c.Log)
            .ToListAsync(cancellationToken);
    }

    private void ProcessAssignmentData(MonthlyProgressResponse response, List<Checklist> checklists)
    {
        // Extract assignments from all checklists
        var allAssignments = checklists
            .SelectMany(c => c.Assignments)
            .ToList();

        // Calculate overall assignment stats
        response.TotalAssignmentsCount = allAssignments.Count;
        response.CompletedAssignmentsCount = allAssignments.Count(a => a.Completed);
        response.CompletionRate = response.TotalAssignmentsCount > 0
            ? Math.Round((double)response.CompletedAssignmentsCount / response.TotalAssignmentsCount * 100, 1)
            : 0;

        // Build template type breakdown
        response.CompletionsByTemplateType = allAssignments
            .Where(a => a.Completed)
            .GroupBy(a => a.Template.Type)
            .ToDictionary(
                g => g.Key.ToString(),
                g => g.Count()
            );
    }

    private void ProcessWeightAndMoodLogs(MonthlyProgressResponse response, List<Checklist> checklists)
    {
        // Extract weight logs
        response.WeightLog = checklists
            .Where(c => c.Log != null && c.Log.Weight.HasValue)
            .Select(c => new WeightLogEntry
            {
                Date = c.StartDate,
                Weight = c.Log.Weight
            })
            .OrderBy(w => w.Date)
            .ToList();

        // Extract mood logs
        response.MoodLog = checklists
            .Where(c => c.Log != null && c.Log.Mood.HasValue)
            .Select(c => new MoodLogEntry
            {
                Date = c.StartDate,
                Mood = c.Log.Mood.Value,
                MoodName = c.Log.Mood.Value.ToString()
            })
            .OrderBy(m => m.Date)
            .ToList();
    }

    private async Task<List<AchievementSummary>> GetAchievementsForMonth(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        return await context.UserAchievements
            .Where(a => a.UserId == userId &&
                   a.EarnedAt >= startDate &&
                   a.EarnedAt <= endDate)
            .Select(a => new AchievementSummary
            {
                Name = a.Name,
                Description = a.Description,
                EarnedAt = a.EarnedAt,
                Type = a.Type
            })
            .ToListAsync(cancellationToken);
    }

    private List<WeeklyProgress> BuildWeeklyProgress(List<Checklist> checklists, DateTime startDate, DateTime endDate)
    {
        var weeklyProgress = new List<WeeklyProgress>();
        var currentDate = startDate;

        // Iterate through each week in the month
        while (currentDate <= endDate)
        {
            // Find the start of the week (assuming Monday is the first day)
            var weekStartDate = currentDate;
            while (weekStartDate.DayOfWeek != DayOfWeek.Monday && weekStartDate > startDate)
            {
                weekStartDate = weekStartDate.AddDays(-1);
            }

            var weekEndDate = weekStartDate.AddDays(6);

            // Find checklists that overlap with this week
            var weekChecklists = checklists
                .Where(c => c.StartDate <= weekEndDate && c.StartDate.AddDays(6) >= weekStartDate)
                .ToList();

            var weekAssignments = weekChecklists
                .SelectMany(c => c.Assignments)
                .ToList();

            var totalCount = weekAssignments.Count;
            var completedCount = weekAssignments.Count(a => a.Completed);
            var completionRate = totalCount > 0
                ? Math.Round((double)completedCount / totalCount * 100, 1)
                : 0;

            weeklyProgress.Add(new WeeklyProgress
            {
                WeekStartDate = weekStartDate,
                TotalAssignments = totalCount,
                CompletedAssignments = completedCount,
                CompletionRate = completionRate
            });

            // Move to the next week
            currentDate = weekStartDate.AddDays(7);
        }

        return weeklyProgress;
    }

    private async Task<WorkoutMetricsSummary> GetWorkoutMetrics(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        // Get completed workout assignments for the month
        var workoutAssignments = await context.Assignments
            .Where(a => a.Checklist.UserId == userId &&
                   a.Checklist.StartDate <= endDate &&
                   a.Checklist.StartDate.AddDays(6) >= startDate &&
                   a.Template.Type == TemplateType.Workout &&
                   a.Completed)
            .Select(a => a.Id)
            .ToListAsync(cancellationToken);

        // Get workout metrics
        var workoutItems = await context.WorkoutActivityAssignments
            .Where(w => workoutAssignments.Contains(w.AssignmentId) && w.Completed)
            .Include(w => w.Sets)
            .Include(w => w.WorkoutActivity)
            .ToListAsync(cancellationToken);

        // Calculate workout metrics
        return new WorkoutMetricsSummary
        {
            TotalWorkouts = workoutAssignments.Count,
            TotalSets = workoutItems.Sum(w => w.Sets.Count),
            TotalExercises = workoutItems.Count,
            PopularExercises = workoutItems
                .GroupBy(w => w.WorkoutActivity.ActivityName)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new ExerciseFrequency
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToList(),
            AverageWeightByExercise = workoutItems
                .Where(w => w.Sets.Any(s => s.Weight.HasValue))
                .GroupBy(w => w.WorkoutActivity.ActivityName)
                .Select(g => new ExerciseWeight
                {
                    Name = g.Key,
                    AverageWeight = g.SelectMany(w => w.Sets)
                        .Where(s => s.Weight.HasValue)
                        .Average(s => s.Weight.Value)
                })
                .ToList()
        };
    }

    private async Task<FitnessMetricsSummary> GetFitnessMetrics(
        Guid userId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        // Get completed fitness assignments for the month
        var fitnessAssignments = await context.Assignments
            .Where(a => a.Checklist.UserId == userId &&
                   a.Checklist.StartDate <= endDate &&
                   a.Checklist.StartDate.AddDays(6) >= startDate &&
                   a.Template.Type == TemplateType.Fitness &&
                   a.Completed)
            .Select(a => a.Id)
            .ToListAsync(cancellationToken);

        // Get fitness metrics
        var fitnessItems = await context.FitnessActivityAssignments
            .Where(f => fitnessAssignments.Contains(f.AssignmentId) && f.Completed)
            .Include(f => f.FitnessExercise)
            .ToListAsync(cancellationToken);

        // Calculate fitness metrics
        return new FitnessMetricsSummary
        {
            TotalFitnessActivities = fitnessAssignments.Count,
            TotalDuration = fitnessItems
                .Where(f => f.CompletedDuration.HasValue)
                .Sum(f => f.CompletedDuration.Value),
            PopularActivities = fitnessItems
                .GroupBy(f => f.FitnessExercise.ActivityName)
                .OrderByDescending(g => g.Count())
                .Take(5)
                .Select(g => new ExerciseFrequency
                {
                    Name = g.Key,
                    Count = g.Count()
                })
                .ToList(),
            IntensityDistribution = fitnessItems
                .Where(f => !string.IsNullOrEmpty(f.ActualIntensity))
                .GroupBy(f => f.ActualIntensity)
                .Select(g => new IntensityCount
                {
                    Intensity = g.Key,
                    Count = g.Count()
                })
                .ToList()
        };
    }

    private async Task<UserStreakInfo> GetUserStreakInfo(Guid userId, CancellationToken cancellationToken)
    {
        var streak = await context.UserActivityStreaks
            .FirstOrDefaultAsync(s => s.UserId == userId, cancellationToken);

        if (streak != null)
        {
            return new UserStreakInfo
            {
                CurrentStreak = streak.CurrentStreak,
                LongestStreak = streak.LongestStreak,
                LastActivityDate = streak.LastActivityDate,
                TotalActivitiesCompleted = streak.CompletedActivitiesCount
            };
        }

        return new UserStreakInfo
        {
            CurrentStreak = 0,
            LongestStreak = 0,
            LastActivityDate = DateTime.MinValue,
            TotalActivitiesCompleted = 0
        };
    }
}