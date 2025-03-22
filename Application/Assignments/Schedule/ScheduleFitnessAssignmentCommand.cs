using Application.Abstractions.Messaging;
using System;

namespace Application.Assignments.Schedule
{
    public record ScheduleFitnessAssignmentCommand(
        Guid UserId,
        Guid ChecklistId,
        Guid FitnessTemplateId,
        DayOfWeek ScheduledDay,
        bool IsRecurring = false) : ICommand<Guid>;
}