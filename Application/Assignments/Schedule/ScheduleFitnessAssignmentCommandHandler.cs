using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Helpers;
using Domain.Assignments;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.Schedule
{
    internal sealed class ScheduleFitnessAssignmentCommandHandler(IApplicationDbContext context, IChecklistService checklistService)
        : ICommandHandler<ScheduleFitnessAssignmentCommand, System.Guid>
    {
        public async Task<Result<System.Guid>> Handle(ScheduleFitnessAssignmentCommand command, CancellationToken cancellationToken)
        {
            var checklist = await checklistService.CreateChecklistForDateAsync(command.UserId, command.SelectedDate, cancellationToken);

            // Verify the fitness template exists
            var fitnessTemplate = await context.FitnessTemplates
                .Include(ft => ft.Activities)
                .FirstOrDefaultAsync(t => t.Id == command.FitnessTemplateId, cancellationToken);

            if (fitnessTemplate == null)
            {
                return Result.Failure<System.Guid>(TemplateErrors.TemplateNotFound(command.FitnessTemplateId));
            }

            // Check for existing assignment with same day and type
            var existingAssignment = await context.Assignments
                .Include(a => a.Template)
                .FirstOrDefaultAsync(a =>
                    a.ChecklistId == checklist.Id &&
                    a.ScheduledDay == command.ScheduledDay &&
                    a.Template.Id == command.FitnessTemplateId,
                    cancellationToken);

            if (existingAssignment != null)
            {
                return Result.Failure<System.Guid>(
                    TemplateErrors.TemplateAssignmentExists(checklist.Id, command.ScheduledDay.ToString())
                );
            }

            // Create the assignment
            var assignment = new Assignment
            {
                TemplateId = fitnessTemplate.Id,
                Template = fitnessTemplate,
                ScheduledDay = command.ScheduledDay,
                ChecklistId = checklist.Id,
                IsRecurring = command.IsRecurring,
                RecurringStartDate = checklist.StartDate
            };

            context.Assignments.Add(assignment);
            await context.SaveChangesAsync(cancellationToken);

            foreach (var activity in fitnessTemplate.Activities)
            {
                var fitnessActivityAssignment = new FitnessActivityAssignment
                {
                    AssignmentId = assignment.Id,
                    FitnessActivityId = activity.Id,
                    FitnessExercise = activity
                };

                context.FitnessActivityAssignments.Add(fitnessActivityAssignment);
            }

            await context.SaveChangesAsync(cancellationToken);
            return assignment.Id;
        }
    }
}