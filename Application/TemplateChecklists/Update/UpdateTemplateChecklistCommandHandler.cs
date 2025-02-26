using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.TemplateChecklists.Update;
using Domain.Templates;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.TemplateChecklists.Create;

internal sealed class UpdateTemplateChecklistCommandHandler(IApplicationDbContext context)
      : ICommandHandler<UpdateTemplateChecklistCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTemplateChecklistCommand command, CancellationToken cancellationToken)
    {
        var checklist = await context.WeeklyChecklists
            .Where(tc => tc.UserId == command.UserId)
            .OrderByDescending(tc => tc.StartDay)
            .Include(tc => tc.Assignments) 
            .FirstOrDefaultAsync(cancellationToken);

 
        return checklist.Id;
    }
}
