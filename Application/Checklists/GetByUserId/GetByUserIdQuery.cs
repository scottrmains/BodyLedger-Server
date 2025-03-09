using Application.Abstractions.Messaging;

namespace Application.Checklists.GetByUserId;

public sealed record GetChecklistsByUserIdQuery(Guid UserId, DateTime? ReferenceDate = null) : IQuery<ChecklistsResponse>;
