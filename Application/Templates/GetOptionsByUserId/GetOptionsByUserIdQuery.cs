using Application.Abstractions.Messaging;

namespace Application.Templates.GetOptionsByUserId
{
    public sealed record GetOptionsByUserIdQuery(Guid UserId) : IQuery<TemplateOptionsResponse>;
}
