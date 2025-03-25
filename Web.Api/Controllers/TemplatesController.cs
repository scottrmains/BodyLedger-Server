using Application.Abstractions.Authentication;
using Application.Templates.Delete;
using Application.Templates.Generate;
using Application.Templates.GetAllByUserId;
using Application.Templates.GetById;
using Application.Templates.GetOptionsByUserId;
using Domain.Templates;
using MediatR;

using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using System.Collections.Generic;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Web.Api.Requests.Templates;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplatesController : ControllerBase
    {

        [HttpGet]
        public async Task<IResult> GetTemplates(
               [FromQuery] TemplateType? type,
               ISender sender,
               IUserContext user,
               CancellationToken cancellationToken,
               [FromQuery] int page = 1,
               [FromQuery] int pageSize = 10)
        {
            // One query handles all types
            var query = new GetTemplatesByUserIdQuery(user.UserId, page, pageSize, type);
            var result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        // Unified get by ID endpoint
        [HttpGet("{id}")]
        public async Task<IResult> GetTemplate(
            Guid id,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var query = new GetTemplateByIdQuery(id, user.UserId);
            Result<TemplateResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPost]
        public async Task<IResult> CreateTemplate(
             [FromBody] CreateTemplateRequest request,
             ISender sender,
             IUserContext user,
             CancellationToken cancellationToken)
                {
                    var command = request.CreateCommand(user.UserId);
                    Result<Guid> result = await sender.Send(command, cancellationToken);
                    return result.Match(Results.Ok, CustomResults.Problem);
                }

     
   

        [HttpGet("options")]
        public async Task<IResult> GetTemplateOptions(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var query = new GetOptionsByUserIdQuery(user.UserId);
            Result<Result> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpDelete("{id}/delete")]
        public async Task<IResult> DeleteTemplate(Guid id, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            var query = new DeleteTemplateCommand(id);
            Result<Result> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }


        [HttpPost("generate")]
        public async Task<IResult> GenerateWorkout(
             [FromBody] GenerateWorkoutRequest request,
             ISender sender,
             IUserContext user,
             CancellationToken cancellationToken)
        {
            var command = new GenerateWorkoutTemplateCommand(
                request.Name,
                request.Description,
                user.UserId);

            Result<List<WorkoutActivityResponse>> result = await sender.Send(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        }
    }
}
