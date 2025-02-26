using Application.Abstractions.Authentication;
using Application.TemplateChecklists.Create;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using static Web.Api.Controllers.TemplateChecklistController;
using Web.Api.Infrastructure;
using Web.Api.Extensions;
using Application.Users.GetProfileByUserId;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpGet]
        public async Task<IResult> GetProfile(ISender sender, IUserContext user, CancellationToken cancellationToken)
        {

            var command = new GetProfileByUserIdQuery(user.UserId);
            Result<ProfileResponse> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        //[HttpPost("update-profile")]
        //public async Task<IResult> UpdateProfile(CreateTemplateChecklistRequest request, ISender sender, IUserContext user, CancellationToken cancellationToken)
        //{
        //    var command = new UpdateTemplateChecklistCommand(user.UserId, request.StartDay);
        //    Result<Guid> result = await sender.Send(command, cancellationToken);
        //    return result.Match(Results.Ok, CustomResults.Problem);
        //}
    }
}
