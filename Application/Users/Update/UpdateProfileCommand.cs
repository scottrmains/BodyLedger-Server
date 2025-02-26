using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Update
{
    public sealed record UpdateProfileCommand(Guid UserId, double? CurrentWeight, double? GoalWeight, TimeSpan? CurrentPace, TimeSpan? GoalPace) : ICommand<Guid>;
}
