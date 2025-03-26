using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.DeleteById
{
    internal sealed class DeleteNotificationCommandHandler : ICommandHandler<DeleteNotificationCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserContext _userContext;

        public DeleteNotificationCommandHandler(IApplicationDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Result> Handle(DeleteNotificationCommand command, CancellationToken cancellationToken)
        {
            if (command.UserId != _userContext.UserId)
            {
                return Result.Failure(UserErrors.Unauthorized());
            }

            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == command.NotificationId && n.UserId == command.UserId, cancellationToken);

            if (notification is null)
            {
                return Result.Failure(Error.NotFound(
                    "Notification.NotFound",
                    "The specified notification does not exist or does not belong to the current user."));
            }

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
