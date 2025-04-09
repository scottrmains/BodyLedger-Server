using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assignments;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Security.Cryptography.Xml;

namespace Application.AssignmentItems.Undo;

    internal sealed class UndoAssignmentItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<UndoAssignmentItemCommand>
    {
    public async Task<Result> Handle(UndoAssignmentItemCommand command, CancellationToken cancellationToken)
    {
        var assignmentItem = await context.AssignmentItems
            .Include(x => x.Assignment)
            .FirstOrDefaultAsync(a => a.Id == command.ItemId, cancellationToken);

        if (assignmentItem is null)
        {
            return Result.Failure(Error.NotFound(
                "AssignmentItem.NotFound",
                $"The assignment item with ID {command.ItemId} was not found."
            ));
        }

        // Handle set deletion for workout assignments
        if (assignmentItem is WorkoutActivityAssignment)
        {
            var sets = await context.WorkoutSets
                .Where(s => s.WorkoutActivityAssignmentId == command.ItemId)
                .ToListAsync(cancellationToken);

            if (sets.Any())
            {
                context.WorkoutSets.RemoveRange(sets);
            }
        }

        assignmentItem.UndoCompletion();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}


