using Application.Abstractions.Messaging;


namespace Application.Assignments.Delete;

  public sealed record DeleteAssignmentItemCommand(Guid AssignmentId) : ICommand;

