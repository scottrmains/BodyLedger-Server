using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Admin.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler(IApplicationDbContext context)
       : IQueryHandler<GetAllUsersQuery, List<UserResponse>>
    {
        public async Task<Result<List<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await context.Users
                .AsNoTracking()
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    DateCreated = u.DateCreated,
                    Role = u.Role.ToString()
                })
                .ToListAsync(cancellationToken);

            return users;
        }
    }
}
