using Application.Abstractions.Messaging;
using SharedKernel.Responses;


namespace Application.Templates.GetById;

public sealed record GetTemplateByIdQuery(Guid Id, Guid UserId) : IQuery<TemplateResponse>;
