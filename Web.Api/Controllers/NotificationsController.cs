using Application.Abstractions.Authentication;
using Application.Checklists.GetByUserId;
using Application.Notifications.DeleteById;
using Application.Notifications.MarkAsRead;
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
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetNotifications(
         ISender sender,
         IUserContext user,
         CancellationToken cancellationToken)
        {
           var query = new GetNotificationsByUserIdQuery(user.UserId);
            Result<NotificationsResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpPut("{id}/read")]
        public async Task<IResult> MarkAsRead(
       Guid id,
       ISender sender,
       IUserContext user,
       CancellationToken cancellationToken)
        {
            var command = new MarkNotificationAsReadCommand(id, user.UserId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpPut("read-all")]
        public async Task<IResult> MarkAllAsRead(
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new MarkAllNotificationsAsReadCommand(user.UserId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

        [HttpDelete("{id}")]
        public async Task<IResult> DeleteNotification(
            Guid id,
            ISender sender,
            IUserContext user,
            CancellationToken cancellationToken)
        {
            var command = new DeleteNotificationCommand(id, user.UserId);
            Result<Result> result = await sender.Send(command, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

    }
}
