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

        // Calculate the target cycle start date based on the reference date
        var targetCycleStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, DayOfWeek.Monday, 0);

        // Default start day (used if we don't have a checklist)
        DayOfWeek defaultStartDay = DayOfWeek.Monday;

        // Ensure the checklist exists with recurring assignments (if needed)
        var targetChecklist = await checklistService.EnsureChecklistForWeekWithRecurringAssignments(
            query.UserId, targetCycleStart, cancellationToken);

        // Set default start day from checklist if available
        if (targetChecklist != null)
        {
            defaultStartDay = targetChecklist.StartDay;
        }

        // Get the checklist response for the target cycle
        ChecklistResponse checklistResponse = await checklistService.GetChecklistResponseForCycleAsync(
            query.UserId, targetCycleStart, 0, defaultStartDay, cancellationToken);

        // Get all available date ranges for the calendar
        var dateRanges = await checklistService.GetAvailableDateRanges(query.UserId, cancellationToken);

        // Calculate calendar bounds
        var today = DateTime.UtcNow.Date;
        var oldestValidDate = today.AddDays(-90); // Example: 90 days back
        var minDate = dateRanges.Count > 0
            ? dateRanges.Min(d => d.StartDate)
            : oldestValidDate;
        var maxDate = today.AddDays(12 * 7); // 12 weeks in the future

        var response = new ChecklistsResponse
        {
            Checklist = checklistResponse,
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