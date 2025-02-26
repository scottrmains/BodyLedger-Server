using Application.Abstractions.Messaging;


namespace Application.Workouts.GetAllByUserId;

public sealed record GetWorkoutTemplatesByUserIdQuery(Guid UserId, int Page, int PageSize) : IQuery<WorkoutTemplateListResponse>;
