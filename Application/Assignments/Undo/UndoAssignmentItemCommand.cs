using Application.Abstractions.Messaging;


namespace Application.Assignments.Undo;

  public sealed record UndoAssignmentItemCommand(Guid AssignmentId) : ICommand;

