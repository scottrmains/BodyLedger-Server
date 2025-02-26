using Application.Abstractions.Messaging;

namespace Application.Workouts.Create;

public sealed record CreateWorkoutTemplateCommand(
    string Name,
    string? Description,
    List<WorkoutExerciseRequest> Exercises,
    Guid UserId 
) : ICommand<Guid>;


public sealed record WorkoutExerciseRequest(
    string ExerciseName,
    int RecommendedSets,
    string RepRanges
);