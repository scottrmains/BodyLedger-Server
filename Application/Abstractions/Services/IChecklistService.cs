using Application.Checklists.GetByUserId;
using Domain.Checklists;


namespace Application.Abstractions.Services
{
    public interface IChecklistService
    {
        /// <summary>
        /// Creates a checklist for the next week if it doesn't exist and copies recurring assignments.
        /// </summary>
        Task<Checklist> InitiateNextChecklist(Checklist existingChecklist, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the initial checklist for a user starting from the most recent Monday.
        /// </summary>
        Task<Checklist> InitiateFirstChecklist(Guid userId, CancellationToken cancellationToken);

      
        /// <summary>
        /// Gets available date ranges for checklists, including potential future dates.
        /// </summary>
        Task<List<DateRangeInfo>> GetAvailableDateRanges(Guid userId, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a checklist response for a specific cycle, creating it if needed.
        /// </summary>
        Task<ChecklistResponse?> GetChecklistResponseForCycleAsync(
            Guid userId,
            DateTime referenceDate,
            int cycleOffset,
            DayOfWeek defaultStartDay,
            CancellationToken cancellationToken);

        /// <summary>
        /// Adds recurring assignments to a checklist for the next week.
        /// </summary>
        Task<Checklist> EnsureChecklistForWeekWithRecurringAssignments(
         Guid userId,
         DateTime targetDate,
         CancellationToken cancellationToken);
    }
}
