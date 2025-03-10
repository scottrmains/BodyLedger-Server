using Domain.Templates;
using Domain.Workouts;
using SharedKernel;

namespace Application.Templates.GetById;

public class TemplateResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public string TemplateType { get; init; }

}

public class WorkoutTemplateResponse : TemplateResponse
{
    public List<WorkoutExerciseResponse> Exercises { get; init; } = new();
}


public sealed class WorkoutExerciseResponse
{
    public string ExerciseName { get; init; }
    public int RecommendedSets { get; init; }
    public string RepRanges { get; init; }
}

