using Application.Abstractions.Authentication;
using Application.Checklists.GetByUserId;
using Application.TemplateChecklists.Create;
using Application.TemplateChecklists.GetByUserId;
using Application.TemplateChecklists.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.DataTransferObjects;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChecklistController : ControllerBase
    {
        public sealed record UpdateChecklistStartDateRequest(DayOfWeek NewStartDate);


        [HttpGet]
        public async Task<IResult> GetUserChecklists(
        [FromQuery]DateTime date,
         ISender sender,
         IUserContext user,
         CancellationToken cancellationToken)
        {
            var query = new GetChecklistsByUserIdQuery(user.UserId, date);
            Result<ChecklistsResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }



        public sealed record CreateChecklistRequest(
          DayOfWeek StartDay,
          List<DailyAssignmentMapping> DailyAssignments
        );

        [HttpPost("create")]
        public async Task<IResult> CreateChecklist(
            CreateChecklistRequest request,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new CreateChecklistCommand(
                user.UserId,
                request.StartDay,
                request.DailyAssignments
            );

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }







    }
}
