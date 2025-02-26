using Application.Abstractions.Authentication;
using Application.TemplateAssignments.Complete;
using Application.TemplateAssignments.Schedule;
using Application.Templates.GetOptionsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using static Web.Api.Controllers.TemplateChecklistController;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateAssignmentController : ControllerBase
    {

        public sealed record CompleteAssignmentItemRequest(
            Guid AssignmentId
        );

        public sealed record ScheduleTemplateAssignmentRequest(
            Guid ChecklistId, 
            Guid TemplateId,
            DayOfWeek ScheduledDay,
            bool IsRecurring
);

        [HttpPost("schedule-assignment")]
        public async Task<IResult> ScheduleAssignment(ScheduleTemplateAssignmentRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new ScheduleTemplateAssignmentCommand(
                request.ChecklistId,
                request.TemplateId,
                request.ScheduledDay,
                request.IsRecurring,
                user.UserId
            );
            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpGet("template-options")]
        public async Task<IResult> GetTemplateOptions(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var query = new GetOptionsByUserIdQuery(user.UserId);
            Result<Result> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPost("complete-item")]
        public async Task<IResult> CompleteAssignmentItem(CompleteAssignmentItemRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new CompleteAssignmentItemCommand(request.AssignmentId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }



    }

}
