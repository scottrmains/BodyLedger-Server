using Application.TemplateChecklists.GetByUserId;
using Domain.Checklist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IChecklistService
    {
        /// <summary>
        /// Creates a checklist for the next week if it doesn't exist and copies recurring assignments.
        /// </summary>
        Task<WeeklyChecklist> InitiateNextChecklist(WeeklyChecklist existingChecklist, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the initial checklist for a user starting from the most recent Monday.
        /// </summary>
        Task<WeeklyChecklist> InitiateFirstChecklist(Guid userId, CancellationToken cancellationToken);

      
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
        /// Removes empty future checklists that have no assignments
        /// </summary>
        Task CleanupEmptyFutureChecklists(Guid userId, CancellationToken cancellationToken);
    }
}
