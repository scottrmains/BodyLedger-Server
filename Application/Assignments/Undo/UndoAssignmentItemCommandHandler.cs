using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TemplateAssignments.Undo;

    internal sealed class UndoAssignmentItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<UndoAssignmentItemCommand>
    {
        public async Task<Result> Handle(UndoAssignmentItemCommand command, CancellationToken cancellationToken)
        {
        var assignment = await context.TemplateAssignments
             .Include(a => a.Items)
             .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.AssignmentId));
        }

        assignment.UndoCompletion();

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


