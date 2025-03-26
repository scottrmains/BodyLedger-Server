using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Notifications.MarkAsRead;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Notifications.MarkAsRead;

internal sealed class MarkAllNotificationsAsReadCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : ICommandHandler<MarkAllNotificationsAsReadCommand>
{
    public async Task<Result> Handle(MarkAllNotificationsAsReadCommand command, CancellationToken cancellationToken)
    {
        if (command.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        var unreadNotifications = await context.Notifications
            .Where(n => n.UserId == command.UserId && !n.IsRead)
            .ToListAsync(cancellationToken);

        foreach (var notification in unreadNotifications)
        {
            notification.MarkAsRead();
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}