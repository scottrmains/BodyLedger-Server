
namespace SharedKernel.Responses;

public  class TemplateResponse
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public TemplateType TemplateType { get; init; }

}

public sealed class WorkoutTemplateResponse : TemplateResponse
{
    public List<WorkoutActivityResponse> Activities { get; init; } = new();
}

public sealed class FitnessTemplateResponse : TemplateResponse
{
    public List<FitnessActivityResponse> Activities { get; init; } = new();
}


public sealed class WorkoutActivityResponse
{
    public string ActivityName { get; init; }
    public int RecommendedSets { get; init; }
    public string RepRanges { get; init; }
}


public sealed class FitnessActivityResponse
{
    public string ActivityName { get; init; }
    public int RecommendedDuration { get; init; }
    public string IntensityLevel { get; init; }
}
