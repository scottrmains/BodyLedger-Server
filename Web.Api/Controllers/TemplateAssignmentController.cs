using Application.Abstractions.Authentication;
using Application.Assignments.Complete;
using Application.Assignments.Delete;
using Application.Assignments.Schedule;
using Application.Assignments.Undo;
using Application.Templates.GetOptionsByUserId;
using Domain.Templates;
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

        public sealed record ScheduleTemplateAssignmentRequest(
            Guid ChecklistId, 
            Guid TemplateId,
            DayOfWeek ScheduledDay,
            bool IsRecurring,
            TemplateType TemplateType
            );

        public sealed record UndoItemRequest
        {
            public Guid AssignmentId { get; set; }
        }

        public sealed record CompleteItemRequest
        {
            public Guid AssignmentId { get; set; }
        }

        public sealed record DeleteItemRequest
        {
            public Guid AssignmentId { get; set; }
        }

        [HttpPost("schedule-assignment")]
        public async Task<IResult> ScheduleAssignment(ScheduleTemplateAssignmentRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            Result<Guid> result;
            switch (request.TemplateType)
            {
                case TemplateType.Workout:
                    result = await sender.Send(new ScheduleWorkoutAssignmentCommand(
                        user.UserId,
                        request.ChecklistId,
                        request.TemplateId,
                        request.ScheduledDay,
                        request.IsRecurring
                    ));
                    break;

                //add others here
                default:
                    result = await sender.Send(new ScheduleWorkoutAssignmentCommand(
                          user.UserId,
                          request.ChecklistId,
                          request.TemplateId,
                          request.ScheduledDay,
                          request.IsRecurring
                  ));
                    break;
            }

            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpGet("template-options")]
        public async Task<IResult> GetTemplateOptions(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var query = new GetOptionsByUserIdQuery(user.UserId);
            Result<Result> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPut("complete")]
        public async Task<IResult> CompleteAssignment([FromBody] CompleteItemRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new CompleteAssignmentItemCommand(request.AssignmentId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpPut("undo")]
        public async Task<IResult> UndoAssignment([FromBody]UndoItemRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new UndoAssignmentItemCommand(request.AssignmentId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpDelete("delete")]
        public async Task<IResult> DeleteAssignment([FromBody] DeleteItemRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new DeleteAssignmentItemCommand(request.AssignmentId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }




    }

}
