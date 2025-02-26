using Application.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.GetProfileByUserId;

    public sealed record GetProfileByUserIdQuery(Guid UserId) : IQuery<ProfileResponse>;

