using Domain.Templates;

namespace Web.Api.Requests.Assignments
{
    public sealed record ScheduleAssignmentRequest(
        Guid ChecklistId,
        Guid TemplateId,
        DayOfWeek ScheduledDay,
        bool IsRecurring,
        TemplateType TemplateType
        );
}
