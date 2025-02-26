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
        Task<WeeklyChecklist> InitiateFirstChecklist(Guid userId, CancellationToken cancellationToken);

        Task<WeeklyChecklist> InitiateNextChecklist(WeeklyChecklist existingChecklist, CancellationToken cancellationToken);
        Task<ChecklistResponse?> GetChecklistResponseForCycleAsync(
                                  Guid userId,
                                  DateTime referenceDate,
                                  int cycleOffset,
                                  DayOfWeek defaultStartDay,
                                  CancellationToken cancellationToken);
    }
}
