using Application.Abstractions.Messaging;


namespace Application.Assignments.Complete;

  public sealed record CompleteAssignmentItemCommand(Guid AssignmentId) : ICommand;

