using Application.Abstractions.Messaging;


namespace Application.TemplateAssignments.Undo;

  public sealed record UndoAssignmentItemCommand(Guid AssignmentId) : ICommand;

