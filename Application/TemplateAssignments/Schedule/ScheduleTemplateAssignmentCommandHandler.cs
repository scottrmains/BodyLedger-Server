using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Helpers;
using Domain.Assignments;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TemplateAssignments.Schedule;

internal sealed class ScheduleTemplateAssignmentCommandHandler(IApplicationDbContext context, IChecklistService checklistService)
      : ICommandHandler<ScheduleTemplateAssignmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ScheduleTemplateAssignmentCommand command, CancellationToken cancellationToken)
    {

        var checklist = await context.WeeklyChecklists
               .FirstOrDefaultAsync(c => c.Id == command.ChecklistId && c.UserId == command.UserId, cancellationToken);

        var newTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == command.TemplateId, cancellationToken);
            if (newTemplate == null)
            {
                return Result.Failure<Guid>(TemplateErrors.TemplateNotFound(command.TemplateId));
            }

        var existingAssignment = await context.TemplateAssignments
          .Include(a => a.Template)
          .FirstOrDefaultAsync(a => a.TemplateChecklistId == checklist.Id &&
                                    a.ScheduledDay == command.ScheduledDay &&
                                    a.Template.Type == newTemplate.Type, cancellationToken);

        if (existingAssignment != null)
        {
            return Result.Failure<Guid>(
                TemplateErrors.TemplateAssignmentExists(checklist.Id, command.ScheduledDay.ToString())
            );
        }

        var assignment = new TemplateAssignment
        {
            TemplateId = command.TemplateId,
            ScheduledDay = command.ScheduledDay,
            TemplateChecklistId = checklist.Id,
            IsRecurring = command.IsRecurring
        };

        context.TemplateAssignments.Add(assignment);
        await context.SaveChangesAsync(cancellationToken);

        return assignment.Id;
    }
}

