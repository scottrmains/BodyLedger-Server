using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public sealed record NotificationCreatedDomainEvent(Guid NotificationId) : IDomainEvent;
}
