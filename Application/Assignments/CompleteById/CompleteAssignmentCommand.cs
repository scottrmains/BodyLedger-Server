using Application.Abstractions.Messaging;


namespace Application.Assignments.Complete;

  public sealed record CompleteAssignmentCommand(Guid AssignmentId) : ICommand;

