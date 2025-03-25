using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Responses;

namespace Application.Templates.GetOptionsByUserId;

internal sealed class GetTemplateOptionsByUserIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext)
    : IQueryHandler<GetOptionsByUserIdQuery, TemplateOptionsResponse>
{
    public async Task<Result<TemplateOptionsResponse>> Handle(GetOptionsByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<TemplateOptionsResponse>(UserErrors.Unauthorized());
        }

        // Query templates and include the Type
        var templates = await context.Templates
            .Where(x => x.UserId == query.UserId)
            .Select(x => new TemplateOption
            {
                Id = x.Id,
                Name = x.Name,
                Type = (int)x.Type
            })
            .ToListAsync(cancellationToken);

        var response = new TemplateOptionsResponse
        {
            Templates = templates
        };

        return response;
    }
}
