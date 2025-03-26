using Domain.Assignments;
using Domain.Templates;
using SharedKernel.Enums;

namespace Web.Api.Requests.Assignments
{
    public sealed record ScheduleAssignmentRequest(
        DateTime SelectedDate,
        Guid TemplateId,
        DayOfWeek ScheduledDay,
        TimeOfDay TimeOfDay,
        bool IsRecurring,
        TemplateType TemplateType
        );
}
