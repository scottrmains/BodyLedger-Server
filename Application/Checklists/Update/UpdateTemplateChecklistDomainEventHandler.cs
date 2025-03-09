
using Domain.Checklists;
using MediatR;

namespace Application.Checklists.Update;

internal sealed class ChecklistCreatedDomainEventHandler : INotificationHandler<ChecklistCreatedDomainEvent>
{
    public Task Handle(ChecklistCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }

}