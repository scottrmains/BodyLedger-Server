using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users.Register;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Update;


    internal sealed class UpdateProfileCommandHandler(IApplicationDbContext context)
       : ICommandHandler<UpdateProfileCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(UpdateProfileCommand command, CancellationToken cancellationToken)
        {

        var profile = await context.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == command.UserId, cancellationToken);

        if (profile is null)
        {
            // Create a new profile if it doesn't exist
            profile = new UserProfile { UserId = command.UserId };
            context.UserProfiles.Add(profile);
        }

        profile.CurrentWeight = command.CurrentWeight;
        profile.GoalWeight = command.GoalWeight;
        profile.CurrentPace = command.CurrentPace;
        profile.GoalPace = command.GoalPace;

        await context.SaveChangesAsync(cancellationToken);
        // user.Raise(new UserRegisteredDomainEvent(user.Id));


        return profile.Id;
        }
    }

