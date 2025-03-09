using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.AssignmentItems.Undo;

    internal sealed class UndoAssignmentItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<UndoAssignmentItemCommand>
    {
        public async Task<Result> Handle(UndoAssignmentItemCommand command, CancellationToken cancellationToken)
        {
        var assignmentItem = await context.AssignmentItems
            .Include(x => x.TemplateAssignment)
             .FirstOrDefaultAsync(a => a.Id == command.ItemId, cancellationToken);

      

        if (assignmentItem is null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.ItemId));
        }

        assignmentItem.TemplateAssignment.UndoItemCompletion(command.ItemId);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


