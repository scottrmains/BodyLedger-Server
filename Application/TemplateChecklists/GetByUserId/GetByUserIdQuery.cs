using Application.Abstractions.Messaging;


namespace Application.TemplateChecklists.GetByUserId;

public sealed record GetChecklistsByUserIdQuery(Guid UserId) : IQuery<List<ChecklistResponse>>;
