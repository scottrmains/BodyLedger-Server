using Application.Abstractions.Authentication;
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
    public class TemplateChecklistController : ControllerBase
    {
        public sealed record UpdateChecklistStartDateRequest(DayOfWeek NewStartDate);


        [HttpGet]
        public async Task<IResult> GetChecklists(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new GetChecklistsByUserIdQuery(user.UserId);
            Result<List<ChecklistResponse>> result = await sender.Send(command, cancellationToken);
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
