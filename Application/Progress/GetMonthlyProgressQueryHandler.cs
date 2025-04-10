using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Domain.Users;
using SharedKernel;
using SharedKernel.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Progress.GetMonthlyProgress;

internal sealed class GetMonthlyProgressQueryHandler(
    IProgressService progressService,
    IUserContext userContext)
    : IQueryHandler<GetMonthlyProgressQuery, MonthlyProgressResponse>
{
    public async Task<Result<MonthlyProgressResponse>> Handle(
        GetMonthlyProgressQuery query,
        CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<MonthlyProgressResponse>(UserErrors.Unauthorized());
        }

        // Delegate all the data processing to the progress service
        var response = await progressService.GetMonthlyProgressAsync(
            query.UserId,
            query.Year,
            query.Month,
            cancellationToken);

        return response;
    }
}