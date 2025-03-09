using Application.Abstractions.Messaging;
using Application.Checklists.GetByUserId;


namespace Application.Assignments.GetById;

public sealed record GetAssignmentByIdQuery(Guid Id, Guid UserId) : IQuery<AssignmentResponse>;
