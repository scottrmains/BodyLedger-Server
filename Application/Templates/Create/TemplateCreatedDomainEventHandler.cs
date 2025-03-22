using Domain.Templates;
using MediatR;

namespace Application.Workouts.Create;

internal sealed class TemplateCreatedDomainEventHandler : INotificationHandler<TemplateCreatedDomainEvent>
{
    public Task Handle(TemplateCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email or other notifications etc
        return Task.CompletedTask;
    }
}
