using Application.Abstractions.Messaging;


namespace Application.Assignments.Schedule;

public record ScheduleWorkoutAssignmentCommand(
    Guid UserId,
    Guid ChecklistId,
    Guid WorkoutTemplateId,
    DayOfWeek ScheduledDay,
    bool IsRecurring = false) : ICommand<Guid>;

