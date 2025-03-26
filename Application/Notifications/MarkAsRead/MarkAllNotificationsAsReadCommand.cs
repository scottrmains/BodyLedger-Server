using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Notifications.MarkAsRead
{
    public sealed record MarkAllNotificationsAsReadCommand(Guid UserId) : ICommand;
}
