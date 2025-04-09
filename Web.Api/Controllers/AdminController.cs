using Application.Abstractions.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Admin.GetAllUsers;
using Application.Admin.DeleteById;
using SharedKernel.Responses;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        /// <summary>
        /// Gets all users in the system (Admin only)
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet("users")]
        public async Task<IResult> GetAllUsers(
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var query = new GetAllUsersQuery();
            Result<List<UserResponse>> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        /// <summary>
        /// Deletes a user from the system (Admin only)
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>Success or error result</returns>
        [HttpDelete("users/{id}")]
        public async Task<IResult> DeleteUser(
            Guid id,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new DeleteUserCommand(id);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }
    }
}