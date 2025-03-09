namespace Web.Api.Requests.Assignments
{

    public record StopRecurringAssignmentRequest(
        Guid AssignmentId,
        DateTime EffectiveDate,
        bool IsRecurring
        );

}
