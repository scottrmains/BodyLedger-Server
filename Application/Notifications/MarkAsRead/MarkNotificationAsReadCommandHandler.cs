using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Notifications;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Notifications.MarkAsRead;

internal sealed class MarkNotificationAsReadCommandHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : ICommandHandler<MarkNotificationAsReadCommand>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand command, CancellationToken cancellationToken)
    {
        if (command.UserId != userContext.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        var notification = await context.Notifications
            .FirstOrDefaultAsync(n => n.Id == command.NotificationId && n.UserId == command.UserId, cancellationToken);

        if (notification is null)
        {
            return Result.Failure(Error.NotFound(
                "Notification.NotFound",
                "The specified notification does not exist or does not belong to the current user."));
        }

        notification.MarkAsRead();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}