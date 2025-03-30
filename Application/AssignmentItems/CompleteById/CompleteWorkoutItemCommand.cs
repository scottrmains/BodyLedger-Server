using Application.Abstractions.Messaging;
using SharedKernel.Responses;


namespace Application.AssignmentItems.Complete;

public sealed record CompleteWorkoutItemCommand(
    Guid ItemId,
    Guid AssignmentId,
    Guid UserId,
    List<WorkoutSetDto> WorkoutSets) : ICommand;
