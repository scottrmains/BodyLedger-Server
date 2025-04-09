using Application.Abstractions.Messaging;
using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Admin.GetAllUsers
{
    public sealed record GetAllUsersQuery() : IQuery<List<UserResponse>>;

}
