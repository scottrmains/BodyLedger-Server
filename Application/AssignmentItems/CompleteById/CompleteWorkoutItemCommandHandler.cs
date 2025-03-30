using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assignments;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssignmentItems.Complete;

internal sealed class CompleteWorkoutItemCommandHandler(IApplicationDbContext context)
    : ICommandHandler<CompleteWorkoutItemCommand>
{
    public async Task<Result> Handle(CompleteWorkoutItemCommand command, CancellationToken cancellationToken)
    {
        var workoutItem = await context.WorkoutActivityAssignments
                        .FirstOrDefaultAsync(w => w.Id == command.ItemId, cancellationToken);

        if (workoutItem is null)
        {
            return Result.Failure(Error.NotFound(
                "WorkoutItem.NotFound",
                $"The workout item with ID {command.ItemId} was not found."
            ));
        }
        if (workoutItem.AssignmentId != command.AssignmentId)
        {
            return Result.Failure(Error.Problem(
                "WorkoutItem.InvalidAssignment",
                "The workout item does not belong to the specified assignment."
            ));
        }
        var existingSets = await context.WorkoutSets
            .Where(s => s.WorkoutActivityAssignmentId == command.ItemId)
            .ToListAsync(cancellationToken);

        if (existingSets.Any())
        {
            context.WorkoutSets.RemoveRange(existingSets);
        }
        foreach (var setDto in command.WorkoutSets)
        {
            var workoutSet = new WorkoutSet(
                command.ItemId,
                setDto.SetNumber,
                setDto.Reps,
                setDto.Weight
            );

            context.WorkoutSets.Add(workoutSet);
        }
        workoutItem.MarkCompleted();
        var assignment = await context.Assignments
            .Include(a => a.Items)
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment != null && assignment.Items.All(i => i.Completed))
        {
            assignment.MarkCompleted();
        }
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


