using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using MediatR;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Application.Workouts.Create;
using Application.Abstractions.Authentication;
using Application.Workouts.GetAllByUserId;
using Application.Workouts.GetById;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {

        public sealed record CreateWorkoutTemplateRequest(
            string Name,
            string? Description,
            List<WorkoutExerciseRequest> Exercises
        );


        [HttpGet]
        public async Task<IResult> Get(
               ISender sender,
               IUserContext user,
               CancellationToken cancellationToken,
               [FromQuery] int page = 1,
               [FromQuery] int pageSize = 10)
        {
            var command = new GetWorkoutTemplatesByUserIdQuery(user.UserId, page, pageSize);
            Result<WorkoutTemplateListResponse> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpGet("{id}")]
        public async Task<IResult> Get(
            Guid id,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new GetWorkoutTemplateByIdQuery(id, user.UserId);
            Result<WorkoutTemplateResponse> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPost("create")]
        public async Task<IResult> CreateWorkoutTemplate(CreateWorkoutTemplateRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new CreateWorkoutTemplateCommand(
                request.Name,
                request.Description,
                request.Exercises,
                user.UserId
            );

            Result<Guid> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

    }
}
