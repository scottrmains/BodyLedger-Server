using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IApplicationDbContext context,
    ITokenProvider tokenProvider) : ICommandHandler<RefreshTokenCommand, string>
{
    public async Task<Result<string>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {

        User? user = await context.Users
            .SingleOrDefaultAsync(u => u.RefreshToken == command.RefreshToken, cancellationToken);

        if (user is null || !user.IsRefreshTokenValid())
        {
            return Result.Failure<string>(UserErrors.RefreshTokenInvalid);
        }

        string accessToken = tokenProvider.Create(user);

        return accessToken;
    }
}