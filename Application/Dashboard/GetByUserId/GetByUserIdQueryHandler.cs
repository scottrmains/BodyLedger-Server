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

        // Cleanup any empty future checklists (no assignments) before proceeding
       // await checklistService.CleanupEmptyFutureChecklists(query.UserId, cancellationToken);

        // Ensure we have a checklist for the reference date
        var checklist = await context.WeeklyChecklists
            .Where(c => c.UserId == query.UserId && c.StartDate <= referenceDate)
            .OrderByDescending(c => c.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (checklist is null)
        {
            checklist = await checklistService.InitiateFirstChecklist(query.UserId, cancellationToken);
        }

        // Create next week's checklist based on recurring assignments
        await checklistService.InitiateNextChecklist(checklist, cancellationToken);

        // Get the start day (typically Monday)
        DayOfWeek defaultStartDay = checklist.StartDay;

        // Get checklists for current, previous, and next week
        var currentChecklist = await checklistService.GetChecklistResponseForCycleAsync(
            query.UserId, referenceDate, 0, defaultStartDay, cancellationToken);

        var previousChecklist = await checklistService.GetChecklistResponseForCycleAsync(
            query.UserId, referenceDate, -1, defaultStartDay, cancellationToken);

        var nextChecklist = await checklistService.GetChecklistResponseForCycleAsync(
            query.UserId, referenceDate, +1, defaultStartDay, cancellationToken);

        // Get all available date ranges for the calendar
        var dateRanges = await checklistService.GetAvailableDateRanges(query.UserId, cancellationToken);

        // Calculate calendar bounds
        var today = DateTime.UtcNow.Date;
        var minDate = dateRanges.Count > 0
            ? dateRanges.Min(d => d.StartDate)
            : today;

        var maxDate = today.AddDays(12 * 7); 

        var response = new DashboardResponse
        {
            CurrentChecklist = currentChecklist,
            PreviousChecklist = previousChecklist,
            FutureChecklist = nextChecklist,
            DateRanges = dateRanges,
            CalendarBounds = new CalendarBounds
            {
                MinDate = minDate,
                MaxDate = maxDate
            }
        };

        return response;
    }
}


