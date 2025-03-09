using Domain.Assignments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Assignments.Complete;

internal sealed class AssignmentCompleteDomainEventHandler : INotificationHandler<AssignmentCompletedDomainEvent>
{
    public Task Handle(AssignmentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }

}
