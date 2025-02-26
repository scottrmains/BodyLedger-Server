using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Dashboard.GetByUserId;
using Application.Helpers;
using Application.TemplateChecklists.GetByUserId;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace Application.Dashboard;

internal sealed class GetDashboardByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IChecklistService checklistService)
    : IQueryHandler<GetDashboardByUserIdQuery, DashboardResponse>
{
    public async Task<Result<DashboardResponse>> Handle(GetDashboardByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<DashboardResponse>(UserErrors.Unauthorized());
        }

        // Use the provided reference date or default to today
        DateTime referenceDate = query.ReferenceDate?.Date ?? DateTime.UtcNow.Date;

        var checklist = await context.WeeklyChecklists
              .Where(c => c.UserId == query.UserId && c.StartDate <= referenceDate)
              .OrderByDescending(c => c.StartDate)
              .FirstOrDefaultAsync(cancellationToken);

        if (checklist is null)
        {
          checklist = await checklistService.InitiateFirstChecklist(query.UserId, cancellationToken);
        }


         await checklistService.InitiateNextChecklist(checklist, cancellationToken);
        

        //maybe have this dynamically set by user?
        DayOfWeek defaultStartDay = checklist.StartDay;
        var currentChecklist = await checklistService.GetChecklistResponseForCycleAsync(query.UserId, referenceDate, 0, defaultStartDay, cancellationToken);
        var previousChecklist = await checklistService.GetChecklistResponseForCycleAsync(query.UserId, referenceDate, -1, defaultStartDay, cancellationToken);
        var nextChecklist = await checklistService.GetChecklistResponseForCycleAsync(query.UserId, referenceDate, +1, defaultStartDay, cancellationToken);


        var response = new DashboardResponse
        {
            CurrentChecklist = currentChecklist,
            PreviousChecklist = previousChecklist,
            FutureChecklist = nextChecklist,
        };

        return response;
    }
}


