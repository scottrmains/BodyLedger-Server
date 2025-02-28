using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Application.Helpers;
using Application.TemplateChecklists.GetByUserId;
using Domain.Assignments;
using Domain.Checklist;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ChecklistService : IChecklistService
    {
        private readonly IApplicationDbContext context;

        public ChecklistService(IApplicationDbContext context)
        {
            this.context = context;
        }


    public async Task<WeeklyChecklist> InitiateNextChecklist(WeeklyChecklist existingChecklist, CancellationToken cancellationToken)
    {
        DateTime nextCycleStart = existingChecklist.StartDate.AddDays(7);
        var nextChecklist = await context.WeeklyChecklists
            .Where(c => c.UserId == existingChecklist.UserId && c.StartDate == nextCycleStart)
            .FirstOrDefaultAsync(cancellationToken);

        var startLimit = TemplateDateHelper.GetStartLimit(existingChecklist.StartDate, existingChecklist.StartDay);

        if (nextChecklist == null)
        {
            nextChecklist = new WeeklyChecklist
            {
                UserId = existingChecklist.UserId,
                StartDay = existingChecklist.StartDay,
                StartDate = nextCycleStart,
                Assignments = existingChecklist.Assignments
                    .Where(x => x.IsRecurring && (x.RecurringStartDate == null || x.RecurringStartDate <= nextCycleStart))
                    .Select(x => new TemplateAssignment
                    {
                        TemplateId = x.TemplateId,
                        ScheduledDay = x.ScheduledDay,
                        IsRecurring = x.IsRecurring,
                        RecurringStartDate = x.RecurringStartDate,
                        TimeOfDay = x.TimeOfDay
                    }).ToList()

            };
            context.WeeklyChecklists.Add(nextChecklist);
            await context.SaveChangesAsync(cancellationToken);
        }
        

        return nextChecklist;
    }



    public async Task<WeeklyChecklist> InitiateFirstChecklist(Guid userId, CancellationToken cancellationToken)
    {
        DateTime today = DateTime.UtcNow.Date;
        int daysSinceLastMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

        if (daysSinceLastMonday < 0)
        {
            daysSinceLastMonday += 7;
        }

        DateTime startDate = today.AddDays(-daysSinceLastMonday);

        var checklist = new WeeklyChecklist
        {
            UserId = userId,
            StartDay = DayOfWeek.Monday,
            StartDate = startDate
        };

        context.WeeklyChecklists.Add(checklist);
        await context.SaveChangesAsync(cancellationToken);

        return checklist;
    }




    /// <summary>
    /// Gets available date ranges for checklists, including potential future dates.
    /// </summary>
    public async Task<List<DateRangeInfo>> GetAvailableDateRanges(Guid userId, CancellationToken cancellationToken)
    {
        var result = new List<DateRangeInfo>();

        // Get all existing checklists for the user
        var existingChecklists = await context.WeeklyChecklists
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.StartDate)
            .ToListAsync(cancellationToken);

        DateTime today = DateTime.UtcNow.Date;

        // Add existing checklists to the date ranges
        foreach (var checklist in existingChecklists)
        {
            result.Add(new DateRangeInfo
            {
                StartDate = checklist.StartDate,
                EndDate = checklist.StartDate.AddDays(6),
                IsCurrent = today >= checklist.StartDate && today <= checklist.StartDate.AddDays(6),
                HasData = checklist.Assignments.Any()
            });
        }

        // Determine furthest future date (12 weeks from now)
        DateTime maxFutureDate = today.AddDays(12 * 7);

        // If we have existing checklists, look at furthest one
        if (existingChecklists.Any())
        {
            var lastChecklist = existingChecklists.Last();

            // Add future date ranges (for up to 12 weeks)
            DateTime nextStartDate = lastChecklist.StartDate.AddDays(7);

            while (nextStartDate <= maxFutureDate)
            {
                result.Add(new DateRangeInfo
                {
                    StartDate = nextStartDate,
                    EndDate = nextStartDate.AddDays(6),
                    IsCurrent = false,
                    HasData = false
                });

                nextStartDate = nextStartDate.AddDays(7);
            }
        }

        return result;
    }

    /// <summary>
    /// Gets a checklist response for a specific cycle, creating it if needed.
    /// </summary>
    public async Task<ChecklistResponse?> GetChecklistResponseForCycleAsync(
        Guid userId,
        DateTime referenceDate,
        int cycleOffset,
        DayOfWeek defaultStartDay,
        CancellationToken cancellationToken)
    {
        // Calculate target cycle start date using helper
        DateTime targetCycleStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, defaultStartDay, cycleOffset);

        // Retrieve the checklist for that cycle with its assignments, templates, and assignment items
        var checklistEntity = await context.WeeklyChecklists.AsNoTracking()
            .Where(c => c.UserId == userId && c.StartDate == targetCycleStart)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.Template)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (checklistEntity == null)
            return null;

        DateTime today = DateTime.UtcNow.Date;
        bool isCurrent = today >= checklistEntity.StartDate &&
                         today < checklistEntity.StartDate.AddDays(7);

        // Map the TemplateChecklist entity to ChecklistResponse DTO
        var response = new ChecklistResponse
        {
            Id = checklistEntity.Id,
            UserId = checklistEntity.UserId,
            StartDate = checklistEntity.StartDate,
            CompletionPercentage = CalculateCompletionPercentage(checklistEntity),
            IsComplete = checklistEntity.IsComplete,
            StartDay = checklistEntity.StartDay,
            IsCurrent = isCurrent,
            Assignments = checklistEntity.Assignments
                .Select(a => new AssignmentResponse
                {
                    Id = a.Id,
                    TemplateId = a.TemplateId,
                    TemplateName = a.Template.Name,
                    Type = a.Template.Type,
                    ScheduledDay = a.ScheduledDay.ToString(),
                    TimeOfDay = a.TimeOfDay.ToString(),
                    Completed = a.Completed,
                    IsRecurring = a.IsRecurring,
                    RecurringStartDate = a.RecurringStartDate,
                    Items = a.Items
                        .Select(i => new AssignmentItemResponse
                        {
                            Id = i.Id,
                            Completed = i.Completed
                        })
                        .ToList()
                })
                .OrderBy(x => {
                    var day = Enum.Parse<DayOfWeek>(x.ScheduledDay);
                    var startDay = checklistEntity.StartDay;
                    return ((int)day - (int)startDay + 7) % 7;
                })
                .ToList()
        };

        return response;
    }

    // Calculate completion percentage based on completed assignments vs total assignments
    private double CalculateCompletionPercentage(WeeklyChecklist checklist)
    {
        if (!checklist.Assignments.Any())
            return 0;

        int totalAssignments = checklist.Assignments.Count;
        int completedAssignments = checklist.Assignments.Count(a => a.Completed);

        return Math.Round((double)completedAssignments / totalAssignments * 100, 1);
    }

    /// <summary>
    /// Removes empty future checklists that have no assignments
    /// </summary>
    public async Task CleanupEmptyFutureChecklists(Guid userId, CancellationToken cancellationToken)
    {
        DateTime today = DateTime.UtcNow.Date;

        // Find future checklists with no assignments
        var emptyFutureChecklists = await context.WeeklyChecklists
            .Where(c => c.UserId == userId &&
                   c.StartDate > today &&
                   !c.Assignments.Any())
            .ToListAsync(cancellationToken);

        if (emptyFutureChecklists.Any())
        {
            context.WeeklyChecklists.RemoveRange(emptyFutureChecklists);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

}

