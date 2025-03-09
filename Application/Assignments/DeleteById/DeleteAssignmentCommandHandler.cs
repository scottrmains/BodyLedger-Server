using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.Delete;

internal sealed class DeleteAssignmentCommandHandler(IApplicationDbContext context)
        : ICommandHandler<DeleteAssignmentCommand>
    {
        public async Task<Result> Handle(DeleteAssignmentCommand command, CancellationToken cancellationToken)
        {
        var assignment = await context.Assignments
             .Include(a => a.Items)
             .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);

        if (assignment is null)
        {
            return Result.Failure(TemplateErrors.TemplateNotFound(command.AssignmentId));
        }


        context.Assignments.Remove(assignment);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    }


