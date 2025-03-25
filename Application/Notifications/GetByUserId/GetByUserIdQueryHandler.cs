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

namespace Application.TemplateChecklists.GetByUserId;

internal sealed class GetNotificationsByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IChecklistService checklistService)
    : IQueryHandler<GetNotificationsByUserIdQuery, List<NotificationResponse>>
{

    public async Task<Result<List<NotificationResponse>>> Handle(GetNotificationsByUserIdQuery query, CancellationToken cancellationToken)
    {
   
        var notifications = context.Notifications.Where(x => x.UserId == query.UserId).Select(n => 
        new NotificationResponse 
        {
            UserId = n.UserId,
            Message = n.Message, 
            CreatedAt = n.CreatedAt, 
            IsRead = n.IsRead }
        ).ToList();

        return notifications;
    }

}