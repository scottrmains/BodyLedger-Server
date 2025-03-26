using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Responses
{
    public sealed record NotificationsResponse(
        List<NotificationDto> Notifications,
        int UnreadCount,
        int TotalCount);

    public sealed record NotificationDto(
        Guid Id,
        string Title,
        string Message,
        string Type,
        bool IsRead,
        DateTime CreatedAt,
        Dictionary<string, string> Metadata);
}
