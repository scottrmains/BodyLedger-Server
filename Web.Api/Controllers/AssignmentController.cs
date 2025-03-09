using Application.Abstractions.Authentication;
using Application.Assignments.Complete;
using Application.Assignments.Delete;
using Application.Assignments.GetById;
using Application.Assignments.SetRecurring;
using Application.Assignments.Undo;
using Application.Checklists.GetByUserId;
using Application.Templates.GetOptionsByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Web.Api.Requests.Assignments;


namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignmentController : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<IResult> GetAssignment(Guid id, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            var query = new GetAssignmentByIdQuery(id, user.UserId);
            Result<AssignmentResponse> result = await sender.Send(query, cancellationToken);

            if (result.IsFailure)
                return CustomResults.Problem(result);
            return Results.Ok(result.Value);
        }

        [HttpPost("schedule")]
        public async Task<IResult> ScheduleAssignment(ScheduleAssignmentRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = AssignmentCommandFactory.CreateScheduleCommand(
                        request.TemplateType,
                        user.UserId,
                        request.ChecklistId,
                        request.TemplateId,
                        request.ScheduledDay,
                        request.IsRecurring
                    );

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }
        [HttpPut("{id}/complete")]
        public async Task<IResult> CompleteAssignment(Guid id, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new CompleteAssignmentCommand(id);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpPut("{id}/undo")]
        public async Task<IResult> UndoAssignment(Guid id, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new UndoAssignmentCommand(id);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpDelete("{id}/delete")]
        public async Task<IResult> DeleteAssignment(Guid id, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new DeleteAssignmentCommand(id);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpPut("set-recurring")]
        public async Task<IResult> SetRecurringAssignment([FromBody] StopRecurringAssignmentRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            var command = new SetRecurringAssignmentCommand(
                user.UserId,
                request.AssignmentId,
                request.EffectiveDate.ToUniversalTime(),
                request.IsRecurring);

            Result<Result> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }
    }




}


