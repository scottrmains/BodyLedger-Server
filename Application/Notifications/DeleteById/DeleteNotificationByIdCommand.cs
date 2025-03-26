using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.DeleteById
{
    public sealed record DeleteNotificationCommand(Guid NotificationId, Guid UserId) : ICommand;
}
