using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Application.Helpers;
using Application.Abstractions.Services;
using Application.Checklists.GetByUserId;
using SharedKernel.Responses;
using System.Text.Json;

namespace Application.TemplateChecklists.GetByUserId;

internal sealed class GetNotificationsByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IChecklistService checklistService)
    : IQueryHandler<GetNotificationsByUserIdQuery, NotificationsResponse>
{

    public async Task<Result<NotificationsResponse>> Handle(GetNotificationsByUserIdQuery query, CancellationToken cancellationToken)
    {

        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<NotificationsResponse>(UserErrors.Unauthorized());
        }

        var notificationsQuery = context.Notifications
            .Where(n => n.UserId == query.UserId);

        if (query.Type.HasValue)
        {
            notificationsQuery = notificationsQuery.Where(n => n.Type == query.Type.Value);
        }

        int totalCount = await notificationsQuery.CountAsync(cancellationToken);
        int unreadCount = await notificationsQuery.CountAsync(n => !n.IsRead, cancellationToken);

        var notifications = await notificationsQuery
            .OrderByDescending(n => n.CreatedAt)
            .Take(50) 
            .ToListAsync(cancellationToken);
        var notificationDtos = notifications.Select(n => new NotificationDto(
            n.Id,
            n.Title,
            n.Message,
            n.Type.ToString(),
            n.IsRead,
            n.CreatedAt,
            DeserializeMetadata(n.Metadata)
        )).ToList();

        return new NotificationsResponse(
            notificationDtos,
            unreadCount,
            totalCount
        );
    }

    private Dictionary<string, string> DeserializeMetadata(JsonDocument? metadata)
    {
        if (metadata == null)
            return new Dictionary<string, string>();

        try
        {
            var result = new Dictionary<string, string>();
            foreach (var property in metadata.RootElement.EnumerateObject())
            {
                result[property.Name] = property.Value.ToString();
            }
            return result;
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

}