using Application.Abstractions.Data;
using Domain.Assignments;
using Domain.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Assignments.Complete;

internal sealed class AssignmentCompleteDomainEventHandler(IServiceScopeFactory serviceScopeFactory) : INotificationHandler<AssignmentCompletedDomainEvent>
{


    public async Task Handle(AssignmentCompletedDomainEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var assignment = await dbContext.Assignments
                    .AsNoTracking()
                    .Include(x => x.Checklist)
                    .Include(x => x.Template)
                    .FirstOrDefaultAsync(x => x.Id == notification.AssignmentId, cancellationToken);

                if (assignment != null)
                {
                    string templateName = assignment.Template?.Name ?? "activity";

                    var notificationEntity = Notification.CreateAssignmentCompletedNotification(
                        assignment.Checklist.UserId,
                        "Assignment Completed",
                        $"Congratulations! You've completed your {templateName} assignment.",
                        assignment.Id,
                        assignment.TemplateId);

                    dbContext.Notifications.Add(notificationEntity);
                    await dbContext.SaveChangesAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in domain event handler: {ex.Message}");
            }
        }
    }
}