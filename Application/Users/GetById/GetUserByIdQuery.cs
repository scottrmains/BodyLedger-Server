using Application.Abstractions.Messaging;
using SharedKernel.Responses;

namespace Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
