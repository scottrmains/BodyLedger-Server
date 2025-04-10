using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Checklists.Update
{
    internal sealed class UpdateChecklistLogCommandHandler(
         IApplicationDbContext context)
         : ICommandHandler<UpdateChecklistLogCommand>
    {
        public async Task<Result> Handle(
            UpdateChecklistLogCommand command,
            CancellationToken cancellationToken)
        {
            // First verify that the checklist exists and belongs to the user
            var checklist = await context.Checklists
                .FirstOrDefaultAsync(c =>
                    c.Id == command.ChecklistId &&
                    c.UserId == command.UserId,
                    cancellationToken);

            if (checklist is null)
            {
                return Result.Failure(UserErrors.Unauthorized());
            }

            // Retrieve the log
            var log = await context.ChecklistLogs
                .FirstOrDefaultAsync(cl => cl.ChecklistId == command.ChecklistId, cancellationToken);

            if (log is null)
            {
                return Result.Failure(
                    Error.NotFound(
                        "ChecklistLog.NotFound",
                        "No log found for this checklist."));
            }

            // Update the log
            log.Update(command.Weight, command.Notes, command.Mood);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
