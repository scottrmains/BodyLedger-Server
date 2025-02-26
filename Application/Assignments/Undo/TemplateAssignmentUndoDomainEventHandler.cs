using Domain.Assignments;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.TemplateAssignments.Complete;

internal sealed class TemplateAssignmentUndoDomainEventHandler : INotificationHandler<TemplateAssignmentUndoDomainEvent>
{
    public Task Handle(TemplateAssignmentUndoDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        return Task.CompletedTask;
    }

}
