using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using Domain.Templates;
using Application.Abstractions.Authentication;
using Domain.Users;
using Microsoft.Extensions.Caching.Memory;
using Domain.Workouts;
using Application.Templates.Mapping;

namespace Application.Templates.GetById;

internal sealed class GetTemplateByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext, TemplateMapper mapper)
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
        Dictionary<Guid, List<WorkoutExercise>> workoutExercises = null;

        if (template is WorkoutTemplate)
        {
            var exercises = await context.WorkoutExercises
                .Where(e => e.WorkoutTemplateId == template.Id)
                .ToListAsync(cancellationToken);

            workoutExercises = new Dictionary<Guid, List<WorkoutExercise>>
            {
                { template.Id, exercises }
            };
        }

        // Use the mapper to create the response
        return Result.Success(mapper.MapTemplateToResponse(template, workoutExercises));
   
    }


}
