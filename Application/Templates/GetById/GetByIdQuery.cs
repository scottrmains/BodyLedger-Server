using Application.Abstractions.Messaging;


namespace Application.Templates.GetById;

public sealed record GetTemplateByIdQuery(Guid Id, Guid UserId) : IQuery<TemplateResponse>;
