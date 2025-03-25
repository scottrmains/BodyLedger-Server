using Domain.Assignments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AssignmentItems.Complete;

internal sealed class AssignmentItemCompleteDomainEventHandler : INotificationHandler<AssignmentItemCompletedDomainEvent>
{
    public Task Handle(AssignmentItemCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }

}
