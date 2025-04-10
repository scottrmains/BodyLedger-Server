using SharedKernel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Responses
{
    public class MonthlyProgressResponse
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }

        // Assignment stats
        public int TotalAssignmentsCount { get; set; }
        public int CompletedAssignmentsCount { get; set; }
        public double CompletionRate { get; set; } // Percentage

        // Activity type breakdown
        public Dictionary<string, int> CompletionsByTemplateType { get; set; } = new();

        // Weight tracking (from checklist logs)
        public List<WeightLogEntry> WeightLog { get; set; } = new();

        // Mood tracking (from checklist logs)
        public List<MoodLogEntry> MoodLog { get; set; } = new();

        // Achievement summary
        public List<AchievementSummary> Achievements { get; set; } = new();

        // Weekly completion rates
        public List<WeeklyProgress> WeeklyProgress { get; set; } = new();

        // Workout metrics
        public WorkoutMetricsSummary WorkoutMetrics { get; set; }

        // Fitness metrics
        public FitnessMetricsSummary FitnessMetrics { get; set; }

        // User streak information
        public UserStreakInfo StreakInfo { get; set; }
    }

    public class WeightLogEntry
    {
        public DateTime Date { get; set; }
        public double? Weight { get; set; }
    }

    public class MoodLogEntry
    {
        public DateTime Date { get; set; }
        public MoodType Mood { get; set; }
        public string MoodName { get; set; }
    }

    public class AchievementSummary
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EarnedAt { get; set; }
        public AchievementType Type { get; set; }
    }

    public class WeeklyProgress
    {
        public DateTime WeekStartDate { get; set; }
        public int TotalAssignments { get; set; }
        public int CompletedAssignments { get; set; }
        public double CompletionRate { get; set; } // Percentage
    }

    public class WorkoutMetricsSummary
    {
        public int TotalWorkouts { get; set; }
        public int TotalSets { get; set; }
        public int TotalExercises { get; set; }
        public List<ExerciseFrequency> PopularExercises { get; set; } = new();
        public List<ExerciseWeight> AverageWeightByExercise { get; set; } = new();
    }

    public class FitnessMetricsSummary
    {
        public int TotalFitnessActivities { get; set; }
        public int TotalDuration { get; set; } // In minutes
        public List<ExerciseFrequency> PopularActivities { get; set; } = new();
        public List<IntensityCount> IntensityDistribution { get; set; } = new();
    }

    public class ExerciseFrequency
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class ExerciseWeight
    {
        public string Name { get; set; }
        public double AverageWeight { get; set; }
    }

    public class IntensityCount
    {
        public string Intensity { get; set; }
        public int Count { get; set; }
    }

    public class UserStreakInfo
    {
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime LastActivityDate { get; set; }
        public int TotalActivitiesCompleted { get; set; }
    }
}
