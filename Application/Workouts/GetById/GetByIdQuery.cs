using Application.Abstractions.Messaging;


namespace Application.Workouts.GetById;

public sealed record GetWorkoutTemplateByIdQuery(Guid Id, Guid UserId) : IQuery<WorkoutTemplateResponse>;
