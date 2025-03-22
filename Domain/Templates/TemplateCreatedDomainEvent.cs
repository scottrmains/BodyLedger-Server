using SharedKernel;

namespace Domain.Templates;

public sealed record TemplateCreatedDomainEvent(Guid WorkoutTemplateId, TemplateType templateType) : IDomainEvent;
