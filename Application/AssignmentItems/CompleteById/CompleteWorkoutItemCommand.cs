using Application.Abstractions.Messaging;


namespace Application.AssignmentItems.Complete;

  public sealed record CompleteWorkoutItemCommand(Guid ItemId, Guid AssignmentId, Guid UserId, int Sets, int Reps, int? Weight) : ICommand;

