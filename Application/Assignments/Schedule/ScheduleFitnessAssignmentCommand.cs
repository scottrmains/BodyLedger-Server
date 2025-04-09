using Application.Abstractions.Messaging;
using Domain.Assignments;
using SharedKernel.Enums;
using System;

namespace Application.Assignments.Schedule
{
    public record ScheduleFitnessAssignmentCommand(
        Guid UserId,
        DateTime SelectedDate,
        Guid FitnessTemplateId,
        DayOfWeek ScheduledDay,
        TimeOfDay TimeOfDay,
        bool IsRecurring = false) : ICommand<Guid>;
}