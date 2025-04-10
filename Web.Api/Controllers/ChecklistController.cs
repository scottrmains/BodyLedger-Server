using Application.Abstractions.Authentication;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Responses;
using SharedKernel;
using Web.Api.Infrastructure;
using Web.Api.Extensions;
using Application.Checklists.GetByUserId;
using Web.Api.Requests.Checklists;
using Application.Checklists.Update;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChecklistController : ControllerBase
    {

        [HttpGet]
        public async Task<IResult> GetUserChecklists(
        [FromQuery] DateTime date,
         ISender sender,
         IUserContext user,
         CancellationToken cancellationToken)
        {
            var query = new GetChecklistByUserIdQuery(user.UserId, date);
            Result<ChecklistResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPut("{checklistId}/log")]
        public async Task<IResult> UpdateChecklistLog(
            Guid checklistId,
            [FromBody] UpdateChecklistLogRequest request,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new UpdateChecklistLogCommand(
                user.UserId,
                checklistId,
                request.Weight,
                request.Notes,
                request.Mood);

            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

    }
}
