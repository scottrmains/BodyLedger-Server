using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

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

        var options = context.Templates.Where(x => x.UserId == query.UserId)
                                        .Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name))
                                        .ToDictionary(x => x.Key, x => x.Value);

        var response = new TemplateOptionsResponse
        {
            TemplateNames = options
        };

        return response;
    }
}
