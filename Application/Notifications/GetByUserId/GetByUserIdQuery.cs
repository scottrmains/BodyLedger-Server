using Application.Abstractions.Messaging;
using Domain.Notifications;
using SharedKernel.Responses;

namespace Application.Checklists.GetByUserId;

public sealed record GetNotificationsByUserIdQuery(Guid UserId, NotificationType? Type = null) : IQuery<NotificationsResponse>;
