using SharedKernel;

namespace Domain.Workouts;

public sealed record WorkoutTemplateCreatedDomainEvent(Guid WorkoutTemplateId) : IDomainEvent;
