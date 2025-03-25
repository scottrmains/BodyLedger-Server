using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Authentication;
using Application.Templates.Mapping;
using Domain.Templates;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates.Fitness;
using SharedKernel.Responses;

namespace Application.Templates.GetById
{
    internal sealed class GetTemplateByIdQueryHandler(
        IApplicationDbContext context,
        IUserContext userContext,
        TemplateMapper mapper)
        : IQueryHandler<GetTemplateByIdQuery, TemplateResponse>
    {
        public async Task<Result<TemplateResponse>> Handle(GetTemplateByIdQuery query, CancellationToken cancellationToken)
        {
            if (query.UserId != userContext.UserId)
            {
                return Result.Failure<TemplateResponse>(UserErrors.Unauthorized());
            }

            // Query the base Templates table to get any template type
            var template = await context.Templates
                .Where(t => t.UserId == query.UserId && t.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (template is null)
            {
                return Result.Failure<TemplateResponse>(TemplateErrors.TemplateNotFound(query.Id));
            }

            // Load related data based on template type
            Dictionary<Guid, List<WorkoutActivity>> workoutActivities = null;
            Dictionary<Guid, List<FitnessActivity>> fitnessActivities = null;

            if (template is WorkoutTemplate)
            {
                var activities = await context.WorkoutActivities
                    .Where(e => e.WorkoutTemplateId == template.Id)
                    .ToListAsync(cancellationToken);

                workoutActivities = new Dictionary<Guid, List<WorkoutActivity>>
                {
                    { template.Id, activities }
                };
            }
            else if (template is FitnessTemplate)
            {
                var activities = await context.FitnessActivities
                    .Where(e => e.FitnessTemplateId == template.Id)
                    .ToListAsync(cancellationToken);

                fitnessActivities = new Dictionary<Guid, List<FitnessActivity>>
                {
                    { template.Id, activities }
                };
            }

            // Use the mapper to create the response
            return Result.Success(mapper.MapTemplateToResponse(template, workoutActivities, fitnessActivities));
        }
    }
}