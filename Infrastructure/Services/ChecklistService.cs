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


        if (nextChecklist == null && nextCycleStart == startLimit)
        {
            nextChecklist = new WeeklyChecklist
            {
                UserId = existingChecklist.UserId,
                StartDay = existingChecklist.StartDay,  
                StartDate = nextCycleStart,
                Assignments = existingChecklist.Assignments
                    .Where(x => x.IsRecurring)
                    .Select(x => new TemplateAssignment
                    {
                        TemplateId = x.TemplateId,
                        ScheduledDay = x.ScheduledDay,
                        IsRecurring = x.IsRecurring
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
            CompletionPercentage = checklistEntity.CompletionPercentage,
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
}

