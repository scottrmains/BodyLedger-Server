﻿using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Assignments;
using Domain.TemplateAssignments;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
namespace Application.Assignments.Schedule;

internal sealed class ScheduleWorkoutAssignmentCommandHandler(IApplicationDbContext context, IChecklistService checklistService)
      : ICommandHandler<ScheduleWorkoutAssignmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ScheduleWorkoutAssignmentCommand command, CancellationToken cancellationToken)
    {

        var checklist = await context.Checklists
           .FirstOrDefaultAsync(c => c.Id == command.ChecklistId && c.UserId == command.UserId, cancellationToken);

        if (checklist == null)
        {
            return Result.Failure<Guid>(TemplateErrors.TemplateChecklistNotFound(command.ChecklistId));
        }

        // Verify the workout template exists
        var workoutTemplate = await context.WorkoutTemplates
            .Include(wt => wt.Activities)
            .FirstOrDefaultAsync(t => t.Id == command.WorkoutTemplateId, cancellationToken);

        if (workoutTemplate == null)
        {
            return Result.Failure<Guid>(TemplateErrors.TemplateNotFound(command.WorkoutTemplateId));
        }

        // Check for existing assignment with same day and type
        var existingAssignment = await context.Assignments
            .Include(a => a.Template)
            .FirstOrDefaultAsync(a =>
                a.ChecklistId == checklist.Id &&
                a.ScheduledDay == command.ScheduledDay &&
                a.Template.Id == command.WorkoutTemplateId,
                cancellationToken);

        if (existingAssignment != null)
        {
            return Result.Failure<Guid>(
                TemplateErrors.TemplateAssignmentExists(checklist.Id, command.ScheduledDay.ToString())
            );
        }

        // Create the template assignment
        var assignment = new Assignment
        {
            TemplateId = workoutTemplate.Id,
            Template = workoutTemplate,
            ScheduledDay = command.ScheduledDay,
            ChecklistId = checklist.Id,
            IsRecurring = command.IsRecurring,
            RecurringStartDate = checklist.StartDate
            
        };

        context.Assignments.Add(assignment);
        await context.SaveChangesAsync(cancellationToken); 

        foreach (var activity in workoutTemplate.Activities)
        {
            var workoutActivityAssignment = new WorkoutActivityAssignment
            {
                TemplateAssignmentId = assignment.Id,
                WorkoutActivityId = activity.Id,
                WorkoutActivity = activity
            };

            context.WorkoutActivityAssignments.Add(workoutActivityAssignment);
        }

        await context.SaveChangesAsync(cancellationToken);
        return assignment.Id;
    }
}

