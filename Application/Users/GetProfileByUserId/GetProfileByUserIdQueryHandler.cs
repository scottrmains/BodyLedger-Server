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
            .Include(p => p.User)
            .Where(u => u.UserId == query.UserId)
            .Select(u => new ProfileResponse
            {
                UserId = u.UserId,
                CurrentPace = u.CurrentPace,
                GoalPace = u.GoalPace,
                CurrentWeight = u.CurrentWeight,
                GoalWeight = u.GoalWeight,
                Email = u.User.Email,
                FirstName = u.User.FirstName,
                LastName = u.User.LastName
            })
            .SingleOrDefaultAsync(cancellationToken);

        if (profile is null)
        {
            // Create a new profile with default values
            var newProfile = new UserProfile
            {
                UserId = query.UserId,
                CurrentWeight = 0,
                GoalWeight = 0
            };

            context.UserProfiles.Add(newProfile);
            await context.SaveChangesAsync(cancellationToken);

            // Get the user associated with this profile
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Id == query.UserId, cancellationToken);

            if (user == null)
            {
                return Result.Failure<ProfileResponse>(UserErrors.NotFound(query.UserId));
            }

            profile = new ProfileResponse
            {
                UserId = newProfile.UserId,
                CurrentPace = newProfile.CurrentPace,
                GoalPace = newProfile.GoalPace,
                CurrentWeight = newProfile.CurrentWeight,
                GoalWeight = newProfile.GoalWeight,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        return profile;
    }

}