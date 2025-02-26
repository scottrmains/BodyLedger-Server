using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Application.Helpers;

namespace Application.TemplateChecklists.GetByUserId;

internal sealed class GetChecklistsByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetChecklistsByUserIdQuery, List<ChecklistResponse>>
{
    public async Task<Result<List<ChecklistResponse>>> Handle(GetChecklistsByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<List<ChecklistResponse>>(UserErrors.Unauthorized());
        }

        var checklists = await context.WeeklyChecklists
            .Where(tc => tc.UserId == query.UserId)
            .Include(tc => tc.Assignments)
                .ThenInclude(a => a.Template) 
            .Select(tc => new ChecklistResponse
            {
                Id = tc.Id,
                UserId = tc.UserId,
                StartDate = tc.DateCreated,
                CompletionPercentage = tc.CompletionPercentage,
                IsComplete = tc.IsComplete,
                Assignments = tc.Assignments.Select(a => new AssignmentResponse
                {
                    Id = a.Id,
                    TemplateId = a.TemplateId,
                    TemplateName = a.Template.Name,
                    ScheduledDay = a.ScheduledDay.ToString(),
                    Completed = a.Completed,
                    Items = a.Items.Select(i => new AssignmentItemResponse
                    {
                        Id = i.Id,
                        Completed = i.Completed
                    }).ToList()
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        if (checklists is null)
        {
            return Result.Failure<List<ChecklistResponse>>(TemplateErrors.TemplateNotFound(query.UserId)); ;
        }

        return checklists;
    }
}
