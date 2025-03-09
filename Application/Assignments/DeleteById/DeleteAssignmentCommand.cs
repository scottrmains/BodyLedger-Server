using Application.Abstractions.Messaging;


namespace Application.Assignments.Delete;

  public sealed record DeleteAssignmentCommand(Guid AssignmentId) : ICommand;

