
using Application.Abstractions.Authentication;
using Application.Progress;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using SharedKernel.Responses;
using Web.Api.Extensions;
using Web.Api.Infrastructure;

namespace BodyLedger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProgressController : ControllerBase
    {

        [HttpGet("monthly")]
        public async Task<IResult> GetMonthlyProgress(
                [FromQuery] int? year,
                [FromQuery] int? month,
                ISender sender,
                IUserContext user,
                CancellationToken cancellationToken)
        {

            var today = DateTime.UtcNow;
            int targetYear = year ?? today.Year;
            int targetMonth = month ?? today.Month;

            var query = new GetMonthlyProgressQuery(user.UserId, targetYear, targetMonth);
            Result<MonthlyProgressResponse> result = await sender.Send(query, cancellationToken);
            return result.Match(Results.Ok, CustomResults.Problem);
        }

    }
}
