using Domain.Checklist;
using MediatR;

namespace Application.TemplateChecklists.Update;

internal sealed class TemplateChecklistCreatedDomainEventHandler : INotificationHandler<WeeklyChecklistCreatedDomainEvent>
{
    public Task Handle(WeeklyChecklistCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }

}