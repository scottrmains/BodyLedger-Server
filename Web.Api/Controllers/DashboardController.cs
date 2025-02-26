using Application.Abstractions.Authentication;
using Application.Dashboard.GetByUserId;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace BodyLedger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {

        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IResult> GetDashboard([FromQuery]DateTime date, ISender sender, IUserContext user, CancellationToken cancellationToken)
        {
            var query = new GetDashboardByUserIdQuery(user.UserId, date);
            Result<DashboardResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }
    }
}
