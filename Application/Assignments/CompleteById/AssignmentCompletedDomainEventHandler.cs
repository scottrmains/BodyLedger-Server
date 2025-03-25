using Application.Abstractions.Data;
using Domain.Assignments;
using Domain.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Assignments.Complete;

internal sealed class AssignmentCompleteDomainEventHandler(IApplicationDbContext context) : INotificationHandler<AssignmentCompletedDomainEvent>
{
    public Task Handle(AssignmentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var assignment = context.Assignments
                 .Find(new object[] { notification.AssignmentId }, cancellationToken);

        if (assignment != null)
        {
            var template =  context.Templates.Find(new object[] { assignment.TemplateId }, cancellationToken);
            string templateName = template?.Name ?? "activity";

            var notificationEntity = Notification.CreateAssignmentCompletedNotification(
                assignment.Checklist.UserId,
                "Assignment Completed",
                $"Congratulations! You've completed your {templateName} assignment.",
                assignment.Id,
                assignment.TemplateId);

            context.Notifications.Add(notificationEntity);
            context.SaveChangesAsync(cancellationToken);  
        }

        return Task.CompletedTask;
    }
}
