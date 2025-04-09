using Application.Abstractions.Messaging;
using Application.Users.GetById;
using SharedKernel.Responses;

namespace Application.Users.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenWithUserResponse>;

public class RefreshTokenWithUserResponse
{
    public string AccessToken { get; set; }
    public UserResponse User { get; set; }
}
