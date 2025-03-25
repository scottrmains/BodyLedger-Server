using Application.Checklists.GetByUserId;
using Domain.Checklists;
using SharedKernel.Responses;


namespace Application.Abstractions.Services
{
    public interface IChecklistService
    {

      
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

            Task<Checklist> CreateChecklistForDateAsync(
            Guid userId,
            DateTime referenceDate,
            CancellationToken cancellationToken);
    }
}
