using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.Undo;

    internal sealed class UndoAssignmentCommandHandler(IApplicationDbContext context)
        : ICommandHandler<UndoAssignmentCommand>
    {
        public async Task<Result> Handle(UndoAssignmentCommand command, CancellationToken cancellationToken)
        {
        var assignment = await context.Assignments
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


