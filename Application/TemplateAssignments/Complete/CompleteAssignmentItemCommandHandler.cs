using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.TemplateAssignments.Complete;

    internal sealed class CompleteAssignmentItemCommandHandler(IApplicationDbContext context)
        : ICommandHandler<CompleteAssignmentItemCommand>
    {
        public async Task<Result> Handle(CompleteAssignmentItemCommand command, CancellationToken cancellationToken)
        {
            var assignment = await context.TemplateAssignments
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


