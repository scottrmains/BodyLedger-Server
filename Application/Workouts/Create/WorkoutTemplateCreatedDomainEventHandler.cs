using Domain.Users;
using Domain.Workouts;
using MediatR;

namespace Application.Workouts.Create;

internal sealed class WorkoutTemplateCreatedDomainEventHandler : INotificationHandler<WorkoutTemplateCreatedDomainEvent>
{
    public Task Handle(WorkoutTemplateCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email or other notifications etc
        return Task.CompletedTask;
    }
}
