using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssignmentItems.Complete;

    internal sealed class CompleteWorkoutItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CompleteWorkoutItemCommand>
    {
        public async Task<Result> Handle(CompleteWorkoutItemCommand command, CancellationToken cancellationToken)
        {


        var workoutItem = await context.WorkoutExerciseAssignments
                        .FirstOrDefaultAsync(w => w.Id == command.ItemId, cancellationToken);

        if (workoutItem is null)
        {
            //error
        }

        // Verify this item belongs to the correct assignment
        if (workoutItem.TemplateAssignmentId != command.AssignmentId)
        {
            //error
        }

        // Mark the workout exercise as completed with the specified metrics
        workoutItem.MarkCompleted(
            sets: command.Sets,
            reps: command.Reps,
            weight: command.Weight
        );

        // Get the parent assignment to check if all items are completed
        var assignment = await context.Assignments
            .Include(a => a.Items)
            .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment != null && assignment.Items.All(i => i.Completed))
        {
            // All items are completed, so mark the assignment as completed
            assignment.MarkCompleted();
        }

        // Save all changes
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


