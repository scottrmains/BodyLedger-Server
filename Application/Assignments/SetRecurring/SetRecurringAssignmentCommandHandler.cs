using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.SetRecurring;

internal sealed class SetRecurringAssignmentCommandHandler(IApplicationDbContext context)
        : ICommandHandler<SetRecurringAssignmentCommand>
    {
        public async Task<Result> Handle(SetRecurringAssignmentCommand command, CancellationToken cancellationToken)
        {

        // Find the original assignment
        var assignment = await context.Assignments
            .Include(a => a.Checklist)
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment == null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.AssignmentId));
        }

        // Verify the assignment belongs to the user
        if (assignment.Checklist.UserId != command.UserId)
        {
            return Result.Failure(UserErrors.Unauthorized());
        }

        // Update the original assignment to no longer be recurring
        assignment.IsRecurring = command.SetRecurring;

        // Find all future occurrences of this recurring assignment
        // We need to find assignments with the same template, same day of week, and in future weeks
        var futureAssignments = await context.Assignments
          .Include(a => a.Checklist)
          .Where(a =>
          a.Id != command.AssignmentId &&
          a.TemplateId == assignment.TemplateId &&
          a.ScheduledDay == assignment.ScheduledDay &&
          a.Checklist.UserId == command.UserId &&
          a.Checklist.StartDate >= command.EffectiveDate)
            .ToListAsync(cancellationToken);

        if (command.SetRecurring)
        {
            context.Assignments.AddRange(futureAssignments);

        }
        else
        {
            context.Assignments.RemoveRange(futureAssignments);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


