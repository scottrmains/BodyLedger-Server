using Application.Abstractions.Messaging;
using SharedKernel.Responses;

namespace Application.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
