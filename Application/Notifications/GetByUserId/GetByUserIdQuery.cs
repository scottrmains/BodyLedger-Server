using Application.Abstractions.Messaging;
using SharedKernel.Responses;

namespace Application.Checklists.GetByUserId;

public sealed record GetNotificationsByUserIdQuery(Guid UserId) : IQuery<List<NotificationResponse>>;
