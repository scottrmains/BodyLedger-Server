using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.AssignmentItems.CompleteById;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;


namespace Application.AssignmentItems.Complete
{
    internal sealed class CompleteFitnessItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CompleteFitnessItemCommand>
    {
        public async Task<Result> Handle(CompleteFitnessItemCommand command, CancellationToken cancellationToken)
        {
            var fitnessItem = await context.FitnessActivityAssignments
                .FirstOrDefaultAsync(f => f.Id == command.ItemId, cancellationToken);

            if (fitnessItem is null)
            {
                return Result.Failure(TemplateErrors.TemplateNotFound(command.ItemId));
            }

            // Verify this item belongs to the correct assignment
            if (fitnessItem.TemplateAssignmentId != command.AssignmentId)
            {
                return Result.Failure(TemplateErrors.TemplateNotFound(command.ItemId));
            }

            // Mark the fitness activity as completed with the specified metrics
            fitnessItem.MarkCompleted(
                duration: command.Duration,
                intensity: command.Intensity
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
}