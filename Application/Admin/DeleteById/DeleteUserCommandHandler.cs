using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Admin.DeleteById;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Admin.DeleteById;

internal sealed class DeleteUserCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        // Delete related entities
        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == command.UserId, cancellationToken);

        if (profile is not null)
        {
            context.UserProfiles.Remove(profile);
        }

        // Delete notifications
        var notifications = await context.Notifications
            .Where(n => n.UserId == command.UserId)
            .ToListAsync(cancellationToken);

        if (notifications.Any())
        {
            context.Notifications.RemoveRange(notifications);
        }

        // Delete user achievements
        var achievements = await context.UserAchievements
            .Where(a => a.UserId == command.UserId)
            .ToListAsync(cancellationToken);

        if (achievements.Any())
        {
            context.UserAchievements.RemoveRange(achievements);
        }

        // Delete activity streak if exists
        var streak = await context.UserActivityStreaks
            .FirstOrDefaultAsync(s => s.UserId == command.UserId, cancellationToken);

        if (streak is not null)
        {
            context.UserActivityStreaks.Remove(streak);
        }

        // Finally delete the user
        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}