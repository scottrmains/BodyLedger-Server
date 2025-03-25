using Application.Abstractions.Messaging;
using SharedKernel.Responses;


namespace Application.Assignments.GetById;

public sealed record GetAssignmentByIdQuery(Guid Id, Guid UserId) : IQuery<AssignmentResponse>;
