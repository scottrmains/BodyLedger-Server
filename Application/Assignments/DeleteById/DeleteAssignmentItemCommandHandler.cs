using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.Delete;

internal sealed class DeleteAssignmentItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<DeleteAssignmentItemCommand>
    {
        public async Task<Result> Handle(DeleteAssignmentItemCommand command, CancellationToken cancellationToken)
        {
        var assignment = await context.TemplateAssignments
             .Include(a => a.Items)
             .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.AssignmentId));
        }


        context.TemplateAssignments.Remove(assignment);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


