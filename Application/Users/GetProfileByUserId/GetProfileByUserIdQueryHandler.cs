using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.GetById;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetProfileByUserId;
internal sealed class GetProfileByUserIdQueryHandler(IApplicationDbContext context, IUserContext userContext)
    : IQueryHandler<GetProfileByUserIdQuery, ProfileResponse>
{
    public async Task<Result<ProfileResponse>> Handle(GetProfileByUserIdQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<ProfileResponse>(UserErrors.Unauthorized());
        }

        ProfileResponse? profile = await context.UserProfiles
            .Where(u => u.Id == query.UserId)
            .Select(u => new ProfileResponse
            {
                UserId = u.UserId,  
                CurrentPace = u.CurrentPace,
                GoalPace = u.GoalPace,
                CurrentWeight = u.CurrentWeight,
                GoalWeight = u.GoalWeight
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            return Result.Failure<ProfileResponse>(UserErrors.NotFound(query.UserId));
        }

        return profile;
    }

}