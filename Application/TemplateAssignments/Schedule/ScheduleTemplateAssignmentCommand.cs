using Application.Abstractions.Messaging;


namespace Application.TemplateAssignments.Schedule;

public sealed record ScheduleTemplateAssignmentCommand(Guid ChecklistId, Guid TemplateId,DayOfWeek ScheduledDay, bool IsRecurring, Guid UserId) : ICommand<Guid>;

