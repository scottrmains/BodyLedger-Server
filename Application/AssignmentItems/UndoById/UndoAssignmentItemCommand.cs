using Application.Abstractions.Messaging;


namespace Application.AssignmentItems.Undo;

  public sealed record UndoAssignmentItemCommand(Guid ItemId) : ICommand;

