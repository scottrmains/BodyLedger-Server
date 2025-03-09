using Application.Abstractions.Messaging;


namespace Application.Assignments.SetRecurring;

public record SetRecurringAssignmentCommand(
    Guid UserId,
    Guid AssignmentId,
    DateTime EffectiveDate,
    bool SetRecurring) : ICommand;

