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

namespace Application.TemplateChecklists.GetByUserId;

internal sealed class GetChecklistsByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext,
    IChecklistService checklistService)
    : IQueryHandler<GetChecklistsByUserIdQuery, ChecklistsResponse>
{
    public async Task<Result<ChecklistsResponse>> Handle(GetChecklistsByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<ChecklistsResponse>(UserErrors.Unauthorized());
        }

        // Use the provided reference date or default to today
        DateTime referenceDate = query.ReferenceDate?.Date ?? DateTime.UtcNow.Date;

        // Calculate the dates for current, previous, and next weeks
        var currentWeekStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, DayOfWeek.Monday, 0);
        var previousWeekStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, DayOfWeek.Monday, -1);
        var nextWeekStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, DayOfWeek.Monday, 1);

        // Use the optimized method to ensure checklists exist with recurring assignments
        var currentWeekChecklist = await checklistService.EnsureChecklistForWeekWithRecurringAssignments(
            query.UserId, currentWeekStart, cancellationToken);

        // For previous week, only ensure it exists if it's within the valid range
        ChecklistResponse previousChecklist = null;
        var oldestValidDate = DateTime.UtcNow.Date.AddDays(-90); // Example: 90 days back
        if (previousWeekStart >= oldestValidDate)
        {
            previousChecklist = await checklistService.GetChecklistResponseForCycleAsync(
                query.UserId, previousWeekStart, 0, currentWeekChecklist.StartDay, cancellationToken);
        }

        // For future week, check against max date limit
        ChecklistResponse nextChecklist = null;
        var futureLimit = DateTime.UtcNow.Date.AddDays(12 * 7); // 12 weeks
        if (nextWeekStart <= futureLimit)
        {
            // First ensure the next week checklist exists with recurring assignments
            await checklistService.EnsureChecklistForWeekWithRecurringAssignments(
            query.UserId, nextWeekStart, cancellationToken);

            nextChecklist = await checklistService.GetChecklistResponseForCycleAsync(
                query.UserId, nextWeekStart, 0, currentWeekChecklist.StartDay, cancellationToken);
        }

        // Get the current week's response
        var currentChecklist = await checklistService.GetChecklistResponseForCycleAsync(
            query.UserId, currentWeekStart, 0, currentWeekChecklist.StartDay, cancellationToken);

        // Get all available date ranges for the calendar
        var dateRanges = await checklistService.GetAvailableDateRanges(query.UserId, cancellationToken);

        // Calculate calendar bounds
        var today = DateTime.UtcNow.Date;
        var minDate = dateRanges.Count > 0
            ? dateRanges.Min(d => d.StartDate)
            : oldestValidDate;

        var maxDate = today.AddDays(12 * 7);

        var response = new ChecklistsResponse
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
