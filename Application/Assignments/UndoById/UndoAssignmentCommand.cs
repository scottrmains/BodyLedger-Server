using Application.Abstractions.Messaging;


namespace Application.Assignments.Undo;

  public sealed record UndoAssignmentCommand(Guid AssignmentId) : ICommand;

