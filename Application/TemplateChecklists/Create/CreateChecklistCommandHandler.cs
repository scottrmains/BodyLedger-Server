using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Helpers;
using Domain.Assignments;
using Domain.Checklist;
using Domain.Templates;
using SharedKernel;


namespace Application.TemplateChecklists.Create;

internal sealed class CreateChecklistCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateChecklistCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateChecklistCommand command, CancellationToken cancellationToken)
    {
        var checklist = new WeeklyChecklist
        {
            UserId = command.UserId,
            StartDay = command.StartDay,
            
            StartDate = TemplateDateHelper.GetCycleStartDate(DateTime.UtcNow, command.StartDay)
        };

        context.WeeklyChecklists.Add(checklist);
        await context.SaveChangesAsync(cancellationToken); 

        foreach (var mapping in command.DailyAssignments)
        {
            int offset = TemplateDateHelper.NormalizeDay(mapping.Day, command.StartDay);
            var assignment = new TemplateAssignment
            {
                TemplateId = mapping.TemplateId,
                ScheduledDay = mapping.Day,
                TemplateChecklistId = checklist.Id,
                IsRecurring = mapping.IsRecurring
            };

            context.TemplateAssignments.Add(assignment);
        }

        await context.SaveChangesAsync(cancellationToken);
        return checklist.Id;
    }


}

