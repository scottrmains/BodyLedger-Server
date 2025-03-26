using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Assignments;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Assignments.Complete;

    internal sealed class CompleteAssignmentCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CompleteAssignmentCommand>
    {
        public async Task<Result> Handle(CompleteAssignmentCommand command, CancellationToken cancellationToken)
        {
            var assignment = await context.Assignments
                .Include(a => a.Template)
                .FirstOrDefaultAsync(a => a.Id == command.AssignmentId, cancellationToken);


            if (assignment is null)
            {
                 return Result.Failure<Guid>(TemplateErrors.TemplateNotFound(command.AssignmentId));
            }

            assignment.MarkCompleted();


            await context.SaveChangesAsync(cancellationToken); 

  
        return Result.Success();
        }
    }


