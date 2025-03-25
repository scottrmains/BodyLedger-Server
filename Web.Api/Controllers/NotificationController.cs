using Application.Abstractions.Authentication;
using Application.Checklists.GetByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.Responses;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        public sealed record UpdateChecklistStartDateRequest(DayOfWeek NewStartDate);


        [HttpGet]
        public async Task<IResult> GetNotifications(
         ISender sender,
         IUserContext user,
         CancellationToken cancellationToken)
        {
            var query = new GetNotificationsByUserIdQuery(user.UserId);
            Result<List<NotificationResponse>> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

    }
}
