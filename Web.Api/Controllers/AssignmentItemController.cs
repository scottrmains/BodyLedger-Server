using Application.Abstractions.Authentication;
using Application.AssignmentItems.Undo;
using Application.Assignments.Undo;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Web.Api.Requests.AssignmentItems;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentItemController : ControllerBase
    {

        [HttpPut("{id}/complete")]
        public async Task<IResult> CompleteItem(
           Guid id,
           [FromBody] CompleteItemRequest request,
           ISender sender,
           IUserContext user,
           CancellationToken cancellationToken)
        {
            var command = request.CreateCommand(id, user.UserId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }



        [HttpPut("{id}/undo")]
        public async Task<IResult> UndoItem(
           Guid id,
           ISender sender,
           IUserContext user,
           CancellationToken cancellationToken)
        {
            var command = new UndoAssignmentItemCommand(id);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


    }
}
