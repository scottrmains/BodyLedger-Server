using Application.Abstractions.Messaging;


namespace Application.TemplateAssignments.Complete;

  public sealed record CompleteAssignmentItemCommand(Guid AssignmentId) : ICommand;

