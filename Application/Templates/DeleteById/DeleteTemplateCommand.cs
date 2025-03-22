using Application.Abstractions.Messaging;


namespace Application.Templates.Delete;

  public sealed record DeleteTemplateCommand(Guid TemplateId) : ICommand;

