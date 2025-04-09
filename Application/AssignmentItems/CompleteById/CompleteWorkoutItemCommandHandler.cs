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
            var set = new WorkoutSet(
              workoutItem.Id,
              setDto.SetNumber,
              setDto.Reps,
              setDto.Weight
          );

            context.WorkoutSets.Add(set);
            workoutItem.Sets.Add(set);
        }

        workoutItem.MarkCompleted();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


