using Application.Abstractions.Messaging;
using SharedKernel.Responses;

namespace Application.Templates.GetOptionsByUserId
{
    public sealed record GetOptionsByUserIdQuery(Guid UserId) : IQuery<TemplateOptionsResponse>;
}
