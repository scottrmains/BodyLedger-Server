using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Application.Assignments.Mapping;
using Application.Checklists.GetByUserId;
using Application.Helpers;

using Domain.Assignments;
using Domain.Checklists;
using Domain.TemplateAssignments;
using Domain.Templates;
using Domain.Templates.Fitness;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class ChecklistService : IChecklistService
    {
        private readonly IApplicationDbContext context;
         private readonly AssignmentMapper _mapper;
        public ChecklistService(IApplicationDbContext context, AssignmentMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }


    public async Task<Checklist> InitiateNextChecklist(Checklist existingChecklist, CancellationToken cancellationToken)
    {
        DateTime nextCycleStart = existingChecklist.StartDate.AddDays(7);
        var nextChecklist = await context.Checklists
            .Where(c => c.UserId == existingChecklist.UserId && c.StartDate == nextCycleStart)
            .FirstOrDefaultAsync(cancellationToken);

        var startLimit = TemplateDateHelper.GetStartLimit(existingChecklist.StartDate, existingChecklist.StartDay);
        var today = DateTime.UtcNow.Date;
        var maxDate = today.AddDays(12 * 7);
        if (nextChecklist == null && existingChecklist.StartDate < maxDate)
        {
            nextChecklist = new Checklist
            {
                UserId = existingChecklist.UserId,
                StartDay = existingChecklist.StartDay,
                StartDate = nextCycleStart,
                Assignments = existingChecklist.Assignments
                    .Where(x => x.IsRecurring && (x.RecurringStartDate == null || x.RecurringStartDate <= nextCycleStart))
                    .Select(x => new Assignment
                    {
                        TemplateId = x.TemplateId,
                        ScheduledDay = x.ScheduledDay,
                        IsRecurring = x.IsRecurring,
                        RecurringStartDate = x.RecurringStartDate,
                        TimeOfDay = x.TimeOfDay
                    }).ToList()

            };
            context.Checklists.Add(nextChecklist);
            await context.SaveChangesAsync(cancellationToken);
        }
        

        return nextChecklist;
    }



    public async Task<Checklist> InitiateFirstChecklist(Guid userId, CancellationToken cancellationToken)
    {
        DateTime today = DateTime.UtcNow.Date;
        int daysSinceLastMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;

        if (daysSinceLastMonday < 0)
        {
            daysSinceLastMonday += 7;
        }

        DateTime startDate = today.AddDays(-daysSinceLastMonday);

        var checklist = new Checklist
        {
            UserId = userId,
            StartDay = DayOfWeek.Monday,
            StartDate = startDate
        };

        context.Checklists.Add(checklist);
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
        var existingChecklists = await context.Checklists
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
        var checklistEntity = await context.Checklists.AsNoTracking()
            .Where(c => c.UserId == userId && c.StartDate == targetCycleStart)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.Template)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.Items)
            .FirstOrDefaultAsync(cancellationToken);

        if (checklistEntity == null)
            return null;

        var assignmentIds = checklistEntity.Assignments.Select(a => a.Id).ToList();

        // Load workout activity assignments
        var workoutActivityAssignments = await context.WorkoutActivityAssignments
            .Where(e => assignmentIds.Contains(e.TemplateAssignmentId))
            .Include(e => e.WorkoutActivity)
            .ToListAsync(cancellationToken);

        var fitnessActivityAssignments = await context.FitnessActivityAssignments
            .Where(e => assignmentIds.Contains(e.TemplateAssignmentId))
            .Include(e => e.FitnessExercise)
            .ToListAsync(cancellationToken);

        // Group activity assignments by assignment ID
        var workoutActivityByAssignment = workoutActivityAssignments
            .GroupBy(e => e.TemplateAssignmentId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var fitnessActivitiesByAssignment = fitnessActivityAssignments
            .GroupBy(e => e.TemplateAssignmentId)
            .ToDictionary(g => g.Key, g => g.ToList());

        DateTime today = DateTime.UtcNow.Date;
        bool isCurrent = today >= checklistEntity.StartDate &&
                         today < checklistEntity.StartDate.AddDays(7);

        // Use mapper to create properly typed responses
        var assignments = _mapper.MapAssignmentsToResponses(
                     checklistEntity.Assignments,
                     workoutActivityByAssignment,
                     fitnessActivitiesByAssignment);

        assignments = assignments.OrderBy(x => {
            var day = Enum.Parse<DayOfWeek>(x.ScheduledDay);
            var startDay = checklistEntity.StartDay;
            return ((int)day - (int)startDay + 7) % 7;
        }).ToList();

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
            Assignments = assignments
        };

        return response;
    }

    // Calculate completion percentage based on completed assignments vs total assignments
    private double CalculateCompletionPercentage(Checklist checklist)
    {
        if (!checklist.Assignments.Any())
            return 0;

        int totalAssignments = checklist.Assignments.Count;
        int completedAssignments = checklist.Assignments.Count(a => a.Completed);

        return Math.Round((double)completedAssignments / totalAssignments * 100, 1);
    }

    public async Task<Checklist> EnsureChecklistForWeekWithRecurringAssignments(
        Guid userId,
        DateTime targetDate,
        CancellationToken cancellationToken)
    {
        // Find or create the checklist for this week
        var targetWeekStart = TemplateDateHelper.GetCycleStartForReference(targetDate, DayOfWeek.Monday, 0);
        var existingChecklist = await context.Checklists
            .Include(c => c.Assignments)
            .FirstOrDefaultAsync(c => c.UserId == userId && c.StartDate == targetWeekStart, cancellationToken);

        // If the checklist doesn't exist, create it
        if (existingChecklist == null)
        {
            // Get the most recent checklist before the target date to use as template
            var previousChecklist = await context.Checklists
                .Include(c => c.Assignments)
                    .ThenInclude(a => a.Template)
                .Where(c => c.UserId == userId && c.StartDate < targetWeekStart)
                .OrderByDescending(c => c.StartDate)
                .FirstOrDefaultAsync(cancellationToken);

            if (previousChecklist == null)
            {
                // No previous checklist, create a new empty one
                existingChecklist = new Checklist
                {
                    UserId = userId,
                    StartDay = DayOfWeek.Monday,
                    StartDate = targetWeekStart,
                    Assignments = new List<Assignment>()
                };

                context.Checklists.Add(existingChecklist);
                await context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // Create new checklist based on the previous one
                existingChecklist = new Checklist
                {
                    UserId = userId,
                    StartDay = previousChecklist.StartDay,
                    StartDate = targetWeekStart,
                    Assignments = new List<Assignment>()
                };

                // Add context and save to get a valid ID
                context.Checklists.Add(existingChecklist);
                await context.SaveChangesAsync(cancellationToken);

                // Now add recurring assignments from all previous checklists
                await AddRecurringAssignmentsToChecklist(userId, existingChecklist, cancellationToken);
            }
        }
        else if (!existingChecklist.Assignments.Any())
        {
            // Checklist exists but has no assignments, check if we need to add recurring ones
            await AddRecurringAssignmentsToChecklist(userId, existingChecklist, cancellationToken);
        }

        return existingChecklist;
    }

    private async Task AddRecurringAssignmentsToChecklist(
        Guid userId,
        Checklist targetChecklist,
        CancellationToken cancellationToken)
    {
        // Get all recurring assignments from previous checklists
        var recurringAssignments = await context.Assignments
            .Include(a => a.Template)
            .Include(a => a.Checklist)
            .Where(a =>
                a.Checklist.UserId == userId &&
                a.Checklist.StartDate < targetChecklist.StartDate &&
                a.IsRecurring &&
                (a.RecurringStartDate == null || a.RecurringStartDate <= targetChecklist.StartDate))
            .ToListAsync(cancellationToken);

        if (!recurringAssignments.Any())
        {
            return; // No recurring assignments to add
        }

        // Group by template and scheduled day to avoid duplicates
        var assignmentGroups = recurringAssignments
            .GroupBy(a => new { a.TemplateId, a.ScheduledDay })
            .Select(g => g.OrderByDescending(a => a.Checklist.StartDate).First())
            .ToList();

        // Create new assignments
        var newAssignments = new List<Assignment>();


        foreach (var assignment in assignmentGroups)
        {
            // Skip if this assignment already exists in the target checklist
            if (targetChecklist.Assignments.Any(a =>
                a.TemplateId == assignment.TemplateId &&
                a.ScheduledDay == assignment.ScheduledDay))
            {
                continue;
            }

            // Create a new assignment
            var newAssignment = new Assignment
            {
                TemplateId = assignment.TemplateId,
                ScheduledDay = assignment.ScheduledDay,
                ChecklistId = targetChecklist.Id,
                IsRecurring = assignment.IsRecurring,
                RecurringStartDate = assignment.RecurringStartDate,
                TimeOfDay = assignment.TimeOfDay
            };

            newAssignments.Add(newAssignment);
        }

        // Add all assignments in one batch
        if (newAssignments.Any())
        {
            // Add all new assignments
            context.Assignments.AddRange(newAssignments);
            await context.SaveChangesAsync(cancellationToken);


            foreach (var newAssignment in newAssignments)
            {
  
                if (newAssignment.Template is WorkoutTemplate workoutTemplate)
                {
                    var activities = await context.WorkoutActivities
                        .Where(e => e.WorkoutTemplateId == workoutTemplate.Id)
                        .ToListAsync(cancellationToken);

        
                    foreach (var activity in activities)
                    {
                        var activityAssignment = new WorkoutActivityAssignment
                        {
                            TemplateAssignmentId = newAssignment.Id,
                            WorkoutActivityId = activity.Id
                        };

                        context.WorkoutActivityAssignments.Add(activityAssignment);
                    }
                }
                else if (newAssignment.Template is FitnessTemplate fitnessTemplate)
                {
                    var activities = await context.FitnessActivities
                        .Where(e => e.FitnessTemplateId == fitnessTemplate.Id)
                        .ToListAsync(cancellationToken);

                    foreach (var activity in activities)
                    {
                        var activityAssignment = new FitnessActivityAssignment
                        {
                            TemplateAssignmentId = newAssignment.Id,
                            FitnessActivityId = activity.Id
                        };

                        context.FitnessActivityAssignments.Add(activityAssignment);
                    }
                }
                // Handle other template types as needed
            }

    
            await context.SaveChangesAsync(cancellationToken);
        }
    }

}

