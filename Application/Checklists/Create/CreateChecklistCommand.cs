using Application.Abstractions.Messaging;
using SharedKernel.DataTransferObjects;

namespace Application.TemplateChecklists.Create
{
    public sealed record CreateChecklistCommand(
        Guid UserId,
        DayOfWeek StartDay,
        List<DailyAssignmentMapping> DailyAssignments
    ) : ICommand<Guid>;



}
