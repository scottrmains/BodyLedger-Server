using Application.Abstractions.Messaging;
using Domain.Assignments;


namespace Application.Assignments.Schedule;

public record ScheduleWorkoutAssignmentCommand(
    Guid UserId,
    DateTime SelectedDate,
    Guid WorkoutTemplateId,
    DayOfWeek ScheduledDay,
    TimeOfDay TimeOfDay,
    bool IsRecurring = false) : ICommand<Guid>;

