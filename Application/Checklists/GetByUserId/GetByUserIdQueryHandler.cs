using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Services;
using Application.Helpers;
using Domain.Templates;
using Domain.Users;
using SharedKernel;
using SharedKernel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Checklists.GetByUserId
{

    internal sealed class GetChecklistsByUserIdQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext,
        IChecklistService checklistService)
        : IQueryHandler<GetChecklistByUserIdQuery, ChecklistResponse>
    {
        public async Task<Result<ChecklistResponse>> Handle(GetChecklistByUserIdQuery query, CancellationToken cancellationToken)
        {
            if (query.UserId != userContext.UserId)
            {
                return Result.Failure<ChecklistResponse>(UserErrors.Unauthorized());
            }

            // Use the provided reference date or default to today
            DateTime referenceDate = query.ReferenceDate?.Date ?? DateTime.UtcNow.Date;

            // Calculate the target cycle start date based on the reference date
            var targetCycleStart = TemplateDateHelper.GetCycleStartForReference(referenceDate, DayOfWeek.Monday, 0);

            // Default start day (used if we don't have a checklist)
            DayOfWeek defaultStartDay = DayOfWeek.Monday;

            // Ensure the checklist exists with recurring assignments (if needed)
            var targetChecklist = await checklistService.EnsureChecklistForWeekWithRecurringAssignments(
                query.UserId, targetCycleStart, cancellationToken);

            // Set default start day from checklist if available
            if (targetChecklist != null)
            {
                defaultStartDay = targetChecklist.StartDay;
            }

            // Get the checklist response for the target cycle
            ChecklistResponse? checklistResponse = await checklistService.GetChecklistResponseForCycleAsync(
                query.UserId, targetCycleStart, 0, defaultStartDay, cancellationToken);

            if (checklistResponse == null)
            {
                return Result.Failure<ChecklistResponse>(TemplateErrors.TemplateChecklistNotFound(query.UserId));
            }

            return Result.Success(checklistResponse);
        }
    }
}
