using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;

namespace Application.Templates.Create
{

    public abstract record CreateTemplateCommand(
        string Name,
        string Description,
        Guid UserId
    ) : ICommand<Guid>;


    public sealed record CreateWorkoutTemplateCommand(
        string Name,
        string Description,
        List<WorkoutActivityRequest> Activities,
        Guid UserId
    ) : CreateTemplateCommand(Name, Description, UserId);

    public sealed record CreateFitnessTemplateCommand(
        string Name,
        string Description,
        List<FitnessActivityRequest> Activities,
        Guid UserId
    ) : CreateTemplateCommand(Name, Description, UserId);


    public sealed record WorkoutActivityRequest(
        string ActivityName,
        int RecommendedSets,
        string RepRanges
    );

    public sealed record FitnessActivityRequest(
        string ActivityName,
        int RecommendedDuration,
        string IntensityLevel
    );
}